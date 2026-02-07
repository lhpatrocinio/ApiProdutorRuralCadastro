using AutoMapper;
using ProdutorRuralCadastro.Application.DTOs.Cultura;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Api.Extensions.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Cultura Mappings
            CreateMap<Cultura, CulturaResponse>();

            // Propriedade Mappings
            CreateMap<PropriedadeCreateRequest, Propriedade>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Talhoes, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true));

            CreateMap<PropriedadeUpdateRequest, Propriedade>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProdutorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Talhoes, opt => opt.Ignore());

            CreateMap<Propriedade, PropriedadeResponse>();

            CreateMap<Propriedade, PropriedadeComTalhoesResponse>();

            // Talhao Mappings
            CreateMap<TalhaoCreateRequest, Talhao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Propriedade, opt => opt.Ignore())
                .ForMember(dest => dest.Cultura, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => "Normal"))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true));

            CreateMap<TalhaoUpdateRequest, Talhao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PropriedadeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Propriedade, opt => opt.Ignore())
                .ForMember(dest => dest.Cultura, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDescricao, opt => opt.Ignore());

            CreateMap<Talhao, TalhaoResponse>()
                .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.StatusDescricao) ? src.GetStatusDescricao() : src.StatusDescricao));

            CreateMap<Talhao, TalhaoDetailResponse>()
                .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.StatusDescricao) ? src.GetStatusDescricao() : src.StatusDescricao))
                .ForMember(dest => dest.CulturaNome, opt => opt.MapFrom(src => src.Cultura != null ? src.Cultura.Nome : null))
                .ForMember(dest => dest.PropriedadeNome, opt => opt.MapFrom(src => src.Propriedade != null ? src.Propriedade.Nome : null));

            CreateMap<Talhao, TalhaoResumoResponse>()
                .ForMember(dest => dest.CulturaNome, opt => opt.MapFrom(src => src.Cultura != null ? src.Cultura.Nome : null));
        }
    }
}
