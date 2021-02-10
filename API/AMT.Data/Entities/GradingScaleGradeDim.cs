namespace AMT.Data.Entities
{
    public partial class GradingScaleGradeDim
    {
        public int GradingScaleGradeId { get; set; }
        public int GradingScaleId { get; set; }
        public int Rank { get; set; }
        public string LetterGrade { get; set; }
        public decimal? UpperNumericGrade { get; set; }
    }
}
