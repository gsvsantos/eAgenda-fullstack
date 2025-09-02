using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.Infraestrutura.ORM.Compartilhado;

namespace eAgenda.Infraestrutura.ORM.ModuloContato;

public class RepositorioContatoORM(AppDbContext contexto)
    : RepositorioBaseORM<Contato>(contexto), IRepositorioContato;
