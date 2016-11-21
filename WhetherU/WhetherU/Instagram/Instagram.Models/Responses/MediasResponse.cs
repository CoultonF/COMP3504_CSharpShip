using System;
using System.Collections.Generic;

namespace Instagram.Models.Responses {
    public class MediasResponse : IInstagramResponse {
        public Pagination Pagination { get; set; }
        public Meta Meta { get; set; }
        public List<Media> Data { get; set; }

        public static implicit operator MediasResponse(UserResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
