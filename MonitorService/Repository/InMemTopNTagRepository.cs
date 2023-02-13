using MonitorService.Collections;
using MonitorService.Configuration;
using MonitorService.Dto;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace MonitorService.Repository
{
    /**
     * Using a fixed length Map and MaxPriorityQueue for saving tag counts.
     *  - Fixed length map will have the top N tags by count
     *  - Rest of the tags will be in PQ
     *  - This will avoid sorting a huge collection everytime
     */
    public class InMemTopNTagRepository : ITopNTagRepository
    {
        private readonly FixedSizeDictionary<string, TagDto> topNTags;
        private readonly IndexedPQ<TagDto, string, long> remainingTags;
        private readonly MetricsConfig metricsConfig;


        public InMemTopNTagRepository(MetricsConfig metricsConfig)
        {
            this.metricsConfig = metricsConfig;
            this.topNTags = new FixedSizeDictionary<string, TagDto>(this.metricsConfig.Tags?.TopListSize ?? MetricsConfig.TagsConfig.DEFAULT_TOPLIST_SIZE, 
                (tag) => tag.Tag);
            this.remainingTags = new IndexedPQ<TagDto, string, long>((tag) => tag.Tag, (tag) => tag.count);
        }

        public IReadOnlyDictionary<string, TagDto> getAllCountByTag()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<string, TagDto> getTopNCountByTag()
        {
            return this.topNTags.getReadOnlyDictionary();
        }

        public void incrementTagCount(string tag)
        {
            if (updateTopList(tag))
            {
                return;
            }
            updateRestOfTheTags(tag);
            checkAndPromoteTopOfRemainingList();
        }

        private bool updateTopList(string tag)
        {
            TagDto? tagDto = null;
            if (this.topNTags.TryGetValue(tag, out tagDto))
            {
                tagDto.count++;
                return true;
            }
            return this.topNTags.TryAdd(tag, NewTagDtoInstance());
        }

        private static Func<string, TagDto> NewTagDtoInstance()
        {
            return tag => new TagDto(tag, 1);
        }

        private void updateRestOfTheTags(string tag)
        {
            TagDto? tagDto = this.remainingTags.getByKey(tag);
            if(tagDto != null)
            {
                tagDto.count++;
                this.remainingTags.Enqueue(tagDto);//TODO: adding again because updating priority is not reordering as it is not supported by PriorityQueue<>
            } else
            {
                this.remainingTags.Enqueue(NewTagDtoInstance().Invoke(tag));
            }
        }

        private void checkAndPromoteTopOfRemainingList()
        {
            TagDto? lastOfTopList = getLastTagOfTopList();
            TagDto? topOfRemaining = peekTopOfRemainingList();

            if(lastOfTopList == null || topOfRemaining == null) {
                return;
            }

            if(lastOfTopList?.count < topOfRemaining?.count)
            {
                this.topNTags.TryRemove(lastOfTopList.Tag, out lastOfTopList);
                topOfRemaining = this.remainingTags.Dequeue();

                this.remainingTags.Enqueue(lastOfTopList);
                this.topNTags.TryAdd(topOfRemaining);
            }
        }

        private TagDto? peekTopOfRemainingList()
        {
            return this.remainingTags.peekTopElement();
        }
        private TagDto? getLastTagOfTopList()
        {
            return this.topNTags
                .Values()
                .OrderBy(tag => tag.count)
                .FirstOrDefault();
        }
    }
}
