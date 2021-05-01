using System;

namespace wifimon.TrblApi.Models
{
    public class PutResponseDto
    {
        public bool IsNew {get;set;}

        public override string ToString() => $"{(IsNew ? "New Intel." : "Updated Intel.")}";
    }
}