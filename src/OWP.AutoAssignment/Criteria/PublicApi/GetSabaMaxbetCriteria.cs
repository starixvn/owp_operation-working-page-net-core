using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Criteria.PublicApi
{
    public class GetSabaMaxbetCriteria : CriteriaBase
    {
        public string MatchIds { get; set; }

        public override string GetSettingKey() => "OWP_AutoAssignment_GetSabaMaxbet";

        public override bool IsValid() => true;
    }
}