using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.Compartilhado;

namespace eAgenda.Infraestrutura.ORM.ModuloCompromisso;

public class RepositorioCompromissoORM(AppDbContext contexto)
    : RepositorioBaseORM<Compromisso>(contexto), IRepositorioCompromisso;
