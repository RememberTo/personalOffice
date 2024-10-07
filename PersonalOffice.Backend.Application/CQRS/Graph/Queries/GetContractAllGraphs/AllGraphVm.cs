using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs
{
    /// <summary>
    /// Представление всех значений графика
    /// </summary>
    public class AllGraphVm : IMapWith<AllGraphResult>
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Стоимость портфеля
        /// </summary>
        public decimal PortfolioCost { get; set; }
        /// <summary>
        /// Профит портфеля
        /// </summary>
        public decimal Profitability { get; set; }
        /// <summary>
        /// Свободные средства
        /// </summary>
        public decimal AvailableFunds { get; set; }
        /// <summary>
        /// Вложено в цб vили PortfolioCost - AvailableFunds
        /// </summary>
        public decimal Securities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AllGraphResult, AllGraphVm>()
                .ForMember(x => x.PortfolioCost, opt => opt.MapFrom(res => res.Amount))
                .ForMember(x => x.Profitability, opt => opt.MapFrom(res => res.Profit))
                .ForMember(x => x.AvailableFunds, opt => opt.MapFrom(res => res.Free))
                .ForMember(x => x.Securities, opt => opt.MapFrom(res => res.Sec));

            var b = new B() { IdTemp = 1, NameTemp = "test" };

            var a = new A()
            {
                Id = b.IdTemp,
                Name = b.NameTemp,
            };

        }

        class A
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class B
        {
            public int IdTemp { get; set; }
            public string NameTemp { get; set; }
        }
    }
}
