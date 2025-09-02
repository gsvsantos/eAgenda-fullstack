using eAgenda.Core.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;

public class RepositorioBaseORM<T> where T : EntidadeBase<T>
{
    protected readonly DbSet<T> registros;

    public RepositorioBaseORM(AppDbContext contexto)
    {
        registros = contexto.Set<T>();
    }

    public async Task CadastrarRegistroAsync(T novoRegistro)
    {
        await registros.AddAsync(novoRegistro);
    }

    public async Task CadastrarEntidades(IList<T> entidades)
    {
        await registros.AddRangeAsync(entidades);
    }

    public async Task<bool> EditarRegistroAsync(Guid idRegistro, T registroEditado)
    {
        T? registroSelecionado = await SelecionarRegistroPorIdAsync(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        return true;
    }

    public async Task<bool> ExcluirRegistroAsync(Guid idRegistro)
    {
        T? registroSelecionado = await SelecionarRegistroPorIdAsync(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        return true;
    }

    public virtual async Task<List<T>> SelecionarRegistrosAsync()
    {
        return await registros.ToListAsync();
    }

    public virtual async Task<T?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.FirstOrDefaultAsync(x => x.Id.Equals(idRegistro));
    }
}
