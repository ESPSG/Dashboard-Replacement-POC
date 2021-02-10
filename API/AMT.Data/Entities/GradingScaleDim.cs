using System.Collections.Generic;

namespace AMT.Data.Entities
{
    public partial class GradingScaleDim
    {
        public int GradingScaleId { get; set; }
        public int LocalEducationAgencyId { get; set; }
        public string GradingScaleName { get; set; }

    }
}
