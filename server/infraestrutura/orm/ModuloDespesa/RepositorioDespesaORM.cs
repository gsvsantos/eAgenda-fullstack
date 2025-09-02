using eAgenda.Core.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.ORM.Compartilhado;

namespace eAgenda.Infraestrutura.ORM.ModuloDespesa;

public class RepositorioDespesaORM(AppDbContext contexto)
    : RepositorioBaseORM<Despesa>(contexto), IRepositorioDespesa;
