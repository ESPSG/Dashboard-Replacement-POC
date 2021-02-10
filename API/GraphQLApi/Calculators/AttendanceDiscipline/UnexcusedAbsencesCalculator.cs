using AMT.Data.Common;
using AMT.Data.Descriptors;
using AMT.Data.Descriptors.Enumerations;
using AMT.Data.Entities;
using AMT.Data.Models;
using GraphQLApi.Common;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GraphQLApi.Calculators.AttendanceDiscipline
{
    public class UnexcusedAbsencesCalculator : AbstractAbsencesCalculator
    {
        public UnexcusedAbsencesCalculator(CalculatorAppContext context) :base(context)
        {
           _metricInfo = Common.Metric.StudentTotalUnexcusedDaysAbsent;
            
        }

        protected override int GetAbsences(int excusedAbsences, int unexecusedAbsences)
        {
            return unexecusedAbsences;
        }
    }
}
