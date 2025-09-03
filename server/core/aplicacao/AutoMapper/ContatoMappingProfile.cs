using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.ModuloContato;
using System.Collections.Immutable;

namespace eAgenda.Core.Aplicacao.AutoMapper;

public class ContatoMappingProfile : Profile
{
    public ContatoMappingProfile()
    {
        #region Cadastro
        CreateMap<CadastrarContatoCommand, Contato>();
        CreateMap<Contato, CadastrarContatoResult>();
        #endregion

        #region Edição
        CreateMap<EditarContatoCommand, Contato>();
        CreateMap<Contato, EditarContatoResult>();
        #endregion

        #region SeleçãoPorId
        CreateMap<Contato, SelecionarContatoPorIdResult>()
            .ConvertUsing(src => new SelecionarContatoPorIdResult(
                    src.Id,
                    src.Nome,
                    src.Telefone,
                    src.Email,
                    src.Empresa,
                    src.Cargo,
                    (src.Compromissos ?? new())
                    .Select(r => new DetalhesCompromissoContatoDto(
                            r.Assunto,
                            r.DataOcorrencia,
                            r.HoraInicio,
                            r.HoraTermino
                    )).ToImmutableList()
            ));
        #endregion

        #region SeleçãoTodos
        CreateMap<Contato, SelecionarContatosDto>();

        CreateMap<IEnumerable<Contato>, SelecionarContatosResult>()
            .ConvertUsing((src, dest, ctx) =>
                new SelecionarContatosResult(
                    src?.Select(c => ctx.Mapper.Map<SelecionarContatosDto>(c)).ToImmutableList() ??
                    ImmutableList<SelecionarContatosDto>.Empty
                )
            );
        #endregion
    }
}
