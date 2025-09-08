using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Inital_Config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: true),
                    Empresa = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Despesas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataOcorrencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    FormaPagamento = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PercentualConcluido = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compromissos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Assunto = table.Column<string>(type: "text", nullable: false),
                    DataOcorrencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraTermino = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TipoCompromisso = table.Column<int>(type: "integer", nullable: false),
                    Local = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    ContatoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compromissos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compromissos_Contatos_ContatoId",
                        column: x => x.ContatoId,
                        principalTable: "Contatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoriaDespesa",
                columns: table => new
                {
                    CategoriasId = table.Column<Guid>(type: "uuid", nullable: false),
                    DespesasId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaDespesa", x => new { x.CategoriasId, x.DespesasId });
                    table.ForeignKey(
                        name: "FK_CategoriaDespesa_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaDespesa_Despesas_DespesasId",
                        column: x => x.DespesasId,
                        principalTable: "Despesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensTarefa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TarefaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensTarefa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensTarefa_Tarefas_TarefaId",
                        column: x => x.TarefaId,
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaDespesa_DespesasId",
                table: "CategoriaDespesa",
                column: "DespesasId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Id",
                table: "Categorias",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compromissos_ContatoId",
                table: "Compromissos",
                column: "ContatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Compromissos_Id",
                table: "Compromissos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_Id",
                table: "Contatos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_Id",
                table: "Despesas",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensTarefa_Id",
                table: "ItensTarefa",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensTarefa_TarefaId",
                table: "ItensTarefa",
                column: "TarefaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_Id",
                table: "Tarefas",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaDespesa");

            migrationBuilder.DropTable(
                name: "Compromissos");

            migrationBuilder.DropTable(
                name: "ItensTarefa");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Despesas");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "Tarefas");
        }
    }
}
