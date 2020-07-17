using Data.Models;
using GraphQLApi.Contracts;
using GraphQLApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Repository
{
    public class MetricRepository : IMetricRepository
    {
        private readonly DDSContext _context;
        private readonly IMetricMetadataRepository _metricMetadataRepository;
        public MetricRepository(DDSContext context, IMetricMetadataRepository metricMetadataRepository)
        {
            _context = context;
            _metricMetadataRepository = metricMetadataRepository;
        }

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, int? metricId)
        {
            var metricMetadata = _metricMetadataRepository.GetMetricsMetadata(metricId);
            var metricData = GetStudentMetrics(studentUsi, schoolId);

            var metrics = (from mis in metricMetadata
                           join md in metricData on mis.Id equals md.Id into temp
                           from md in temp.DefaultIfEmpty()
                           select new StudentMetric
                           {
                               Id = mis.Id,
                               Name = mis.Name,
                               ParentId = mis.ParentId,
                               ParentName = mis.ParentName,
                               State = (md?.State),
                               Value = (md?.Value),
                               TrendDirection = (md?.TrendDirection),
                               ValueTypeName = (md?.ValueTypeName),
                               TrendInterpretation = (md?.TrendDirection),
                               Format = (md?.Format),
                               Type = mis.Type
                           });
            return metrics;
        }

        public IEnumerable<StudentMetric> GetStudentMetricsById(int studentUsi, int schoolId, List<int> metricIds)
        {
            var metricInstanceData = GetStudentMetrics(studentUsi, schoolId);

            //Retrieve all the metrics if there are none in the passed in list.
            IQueryable<Data.Models.Metric> metricData = (metricIds == null)
                                                        ? _context.Metric.AsNoTracking().Include(x => x.MetricType).AsNoTracking()
                                                        : _context.Metric.AsNoTracking().Include(x => x.MetricType).AsNoTracking()
                                                            .Where(x => metricIds.Contains(x.MetricId));


            var metrics = (from metricInstance in metricInstanceData
                           join metric in metricData on metricInstance.Id equals metric.MetricId
                           select new StudentMetric
                           {
                               Id = metric.MetricId,
                               Name = metric.MetricName,
                               ParentId = 0,
                               ParentName = "",
                               State = (metricInstance.State),
                               Value = (metricInstance.Value),
                               TrendDirection = (metricInstance.TrendDirection),
                               ValueTypeName = (metricInstance.ValueTypeName),
                               TrendInterpretation = (metricInstance.TrendDirection),
                               Format = (metricInstance.Format),
                               Type = metric.MetricType.MetricTypeName
                           });
            return metrics;
        }
        private List<StudentMetric> GetStudentMetrics(int studentUsi, int schoolId)
        {
            var enhancedStudentInformation = (from si in _context.StudentInformation.AsNoTracking()
                                              join ssi in _context.StudentSchoolInformation.AsNoTracking() on si.StudentUsi equals ssi.StudentUsi
                                              join sci in _context.SchoolInformation.AsNoTracking() on ssi.SchoolId equals sci.SchoolId
                                              where sci.SchoolId == schoolId
                                              select new StudentInformation
                                              {
                                                  StudentUsi = si.StudentUsi,
                                                  SchoolId = ssi.SchoolId,
                                              }).ToList();

            var studentSchoolMetricInstances = _context.StudentSchoolMetricInstanceSet.AsNoTracking().Where(x => x.SchoolId == schoolId && x.StudentUsi == studentUsi);
            List<Guid> studentSchoolMetricInstanceSet = studentSchoolMetricInstances.Select(x => x.MetricInstanceSetKey).ToList();
            var metricInstances = _context.MetricInstance.AsNoTracking().Where(x => studentSchoolMetricInstanceSet.Contains(x.MetricInstanceSetKey));
                                  
            var studentMetrics = (from ehs in enhancedStudentInformation
                                  join ssmi in studentSchoolMetricInstances 
                                  on new { ehs.StudentUsi, ehs.SchoolId } equals new { ssmi.StudentUsi, ssmi.SchoolId }
                                  join mi in metricInstances on ssmi.MetricInstanceSetKey equals mi.MetricInstanceSetKey
                                  join m in _context.Metric.AsNoTracking() on mi.MetricId equals m.MetricId
                                  join mv in _context.MetricVariant.AsNoTracking() on m.MetricId equals mv.MetricId
                                  where ehs.StudentUsi == studentUsi && ehs.SchoolId == schoolId && mv.MetricVariantTypeId == 1
                                  select new StudentMetric
                                  {
                                      StudentUsi = ehs.StudentUsi,
                                      SchoolId = ehs.SchoolId,
                                      State = mi.MetricStateTypeId.HasValue ? MetricUtility.GetMetricState(mi.MetricStateTypeId.Value) : string.Empty,
                                      Value = mi.Value,
                                      TrendDirection = mi.TrendDirection,
                                      ValueTypeName = mi.ValueTypeName,
                                      Id = mi.MetricId,
                                      TrendInterpretation = m.TrendInterpretation,
                                      Format = mv.Format,
                                      Type = MetricUtility.GetMetricTypeName(m.MetricTypeId)
                                  }).ToList();

            foreach (var metric in studentMetrics)
            {
                metric.Value = (!string.IsNullOrEmpty(metric.Format)
                                && !string.IsNullOrEmpty(metric.ValueTypeName)
                                && metric.ValueTypeName.Equals("System.Double")
                                && !string.IsNullOrEmpty(metric.Value)) ? String.Format(metric.Format, Convert.ToDouble(metric.Value)) : metric.Value;
            }

            return studentMetrics;
        }
    }
}