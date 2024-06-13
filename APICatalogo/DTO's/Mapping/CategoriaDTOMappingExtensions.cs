using APICatalogo.Models;
using System.Collections.Generic;

namespace APICatalogo.DTO_s
{
    public static class CategoriaDTOMappingExtensions
    {
        //atribui os valores de categoriaDto à váriavel categoria.
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoriaDto)
        {
            if (categoriaDto is null) return null;

            return new CategoriaDTO()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl,
            };
        }
        //Refaz a conversão, indo para o tipo CategoriaDTO novamente. Isso é feito para podermos ter o tipo de retorno que desejamos, ao invés de um tipo Categoria comum.
        public static Categoria? ToCategoria(this CategoriaDTO categoria)
        {
            if (categoria is null) return null;

            return new Categoria()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };
        }

        public static IEnumerable<CategoriaDTO> ToEnumrableDTO(this IEnumerable<Categoria> categorias)
        {
            if (categorias is null || !categorias.Any())
            {
                return new List<CategoriaDTO>();
            }

            //Aqui, o Select está atribuindo cada item da lista a uma nova instância de CategoriaDTO.
            return categorias.Select(categorias => new CategoriaDTO()
            {
                CategoriaId = categorias.CategoriaId,
                Nome = categorias.Nome,
                ImagemUrl = categorias.ImagemUrl
            });
        }
    }
}
