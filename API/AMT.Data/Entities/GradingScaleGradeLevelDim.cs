using System.ComponentModel.DataAnnotations.Schema;

namespace AMT.Data.Entities
{
    public partial class GradingScaleGradeLevelDim
    {
        public int GradingScaleId { get; set; }
        public int GradeLevelTypeId { get; set; }
        public int GradingScaleGradeLevelId { get; set; }
        
        [NotMapped]
        public string GradeLevelDescription { get; set; }
    }
}
