using BMS_API.Models.DTOs;
using System.Collections.Generic;

namespace BMS_API.Models
{
    public class FeedResponse
    {
        public List<FeedDto> Feeds { get; set; }
        public List<ParticipantDto> Participants { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
