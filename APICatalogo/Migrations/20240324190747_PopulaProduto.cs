using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Coca-Cola Zero','Refrigerante de Cola com zero açucar','7.60','cocacolaz.jpg',50,now(),1)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "" +
                "Values('Chocolate Lacta','Chocolate tradicional Lacta','6.40','chocolacta.jpg',20,now(),3)");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Pizza Portuguesa','Pizza sabor Portuguesa','12.50','pizzaportuguesa.jpg',16,now(),2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produto");
        }
    }
}
