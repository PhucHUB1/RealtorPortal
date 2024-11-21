using System.Text.Json.Serialization;

namespace RealEstate.Dto.Request;

    public class VipPackageSelectionDto
    {
        public int ListingId { get; set; }
        public int VipPackageId { get; set; }

      
        public VipPackageSelectionDto() { }

        
        [JsonConstructor]
        public VipPackageSelectionDto(int listingId, int vipPackageId)
        {
            ListingId = listingId;
            VipPackageId = vipPackageId;
        }
    }
