using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTO_s.Mapping
{
    public class ProdutosDTOMappingExtension : Profile
    {
        public ProdutosDTOMappingExtension()
        {
            CreateMap<Produtos, ProdutosDTO>().ReverseMap();
            CreateMap<Produtos, ProdutosDTOUpdateRequest>().ReverseMap();
            CreateMap<Produtos, ProdutoDTOUpdateResponse>().ReverseMap();
        }
    }
}
