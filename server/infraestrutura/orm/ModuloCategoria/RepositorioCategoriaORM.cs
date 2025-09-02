using eAgenda.Core.Dominio.ModuloCategoria;
using eAgenda.Infraestrutura.ORM.Compartilhado;

namespace eAgenda.Infraestrutura.ORM.ModuloCategoria;

public class RepositorioCategoriaORM(AppDbContext contexto)
    : RepositorioBaseORM<Categoria>(contexto), IRepositorioCategoria;
