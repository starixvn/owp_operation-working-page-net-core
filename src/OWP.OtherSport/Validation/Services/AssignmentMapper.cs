namespace OWP.OtherSport.Validation.Services
{
    using AutoMapper;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public interface AssignmentMapper
    {
        List<Trader> Traders { get; set; }

        int ExtractTrader(string text);

        string ExtractNote(string text);

        string ParseOtherTrader(string otherTrader);

        int GetTraderId(string name);

        void MapHashKey(AssignmentModel model);

        void MapEventDate(AssignmentModelInExcel excelModel, AssignmentModel model);

        List<AssignmentModel> Map(List<AssignmentModelInExcel> excelModels);

        AssignmentModel Map(AssignmentModelInExcel excelModel);

        void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model);

        void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model);
    }

    public class AssignmentMapperImp : AssignmentMapper
    {
        private readonly IMapper mapper;

        private static readonly Regex otherTraderRegex = new Regex(@"#[^\s#]+", RegexOptions.IgnoreCase);

        public AssignmentMapperImp(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public List<Trader> Traders { get; set; }

        public List<AssignmentModel> Map(List<AssignmentModelInExcel> excelModels)
        {
            return excelModels.Select(m => Map(m)).ToList();
        }

        public AssignmentModel Map(AssignmentModelInExcel excelModel)
        {
            var model = mapper.Map<AssignmentModel>(excelModel);
            MapHashKey(model);
            MapEventDate(excelModel, model);
            MapTraders(excelModel, model);
            MapOtherTraders(excelModel, model);
            return model;
        }

        public void MapHashKey(AssignmentModel model)
        {
            model.HashKey = Guid.NewGuid().ToString();
        }

        public void MapEventDate(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            model.DateTime = DateTime.Parse(excelModel.DateTime);
            model.EventDate = model.DateTime;
        }

        public void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            var properties = typeof(AssignmentModelInExcel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("OtherTrader", StringComparison.CurrentCultureIgnoreCase))
                {
                    var property = GetProperty(model, prop.Name);
                    var traderValueInExcel = prop.GetValue(excelModel)?.ToString() ?? string.Empty;
                    var excelTrader = ParseOtherTrader(traderValueInExcel);
                    property.SetValue(model, excelTrader);
                }
            }
        }

        public void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            var properties = typeof(AssignmentModelInExcel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("Trader", StringComparison.CurrentCultureIgnoreCase))
                {
                    var property = GetProperty(model, prop.Name);
                    var traderValueInExcel = prop.GetValue(excelModel)?.ToString() ?? string.Empty;
                    var excelTrader = GetTraderId(traderValueInExcel);
                    property.SetValue(model, excelTrader);
                }
            }
        }

        private static PropertyInfo GetProperty(AssignmentModel model, string propertyName)
        {
            return model.GetType().GetProperties().Single(p => p.Name == propertyName);
        }

        public string ExtractNote(string text)
        {
            var startIndex = text.IndexOf('#');
            if (startIndex == -1)
            {
                return text;
            }
            var endIndex = text.IndexOf(' ', startIndex);
            if (endIndex == -1)
            {
                endIndex = text.Length;
            }

            var note = $"{text.Substring(0, startIndex).Trim()} #<user> {text[endIndex..].Trim()}";
            return $"{note}";
        }

        public int ExtractTrader(string text)
        {
            var matches = otherTraderRegex.Matches(text);
            var name = matches.Cast<Match>().Select(s => s.Value.Remove(0, 1)).FirstOrDefault();

            return string.IsNullOrEmpty(name) ? 0 : GetTraderId(name);
        }

        public int GetTraderId(string name)
        {
            var trader = Traders.FirstOrDefault(t => t.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            return trader?.UserId ?? 0;
        }

        public string ParseOtherTrader(string otherTrader)
        {
            if (string.IsNullOrWhiteSpace(otherTrader))
            {
                return string.Empty;
            }

            var trader = ExtractTrader(otherTrader);
            var note = ExtractNote(otherTrader);
            return $"{trader},{note}";
        }
    }
}