using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.WebAPI.Models.ModuloCompromisso;
using System.Collections.Immutable;

namespace eAgenda.WebAPI.AutoMapper;

public class CompromissoModelsMappingProfile : Profile
{
    public CompromissoModelsMappingProfile()
    {
        #region Cadastrar
        CreateMap<CadastrarCompromissoRequest, CadastrarCompromissoCommand>();
        CreateMap<CadastrarCompromissoResult, CadastrarCompromissoResponse>();
        #endregion

        #region Edição
        CreateMap<(Guid, EditarCompromissoRequest), EditarCompromissoCommand>()
            .ConvertUsing(src => new EditarCompromissoCommand(
                src.Item1,
                src.Item2.Assunto,
                src.Item2.DataOcorrencia,
                src.Item2.HoraInicio,
                src.Item2.HoraTermino,
                src.Item2.TipoCompromisso,
                src.Item2.Local,
                src.Item2.Link,
                src.Item2.ContatoId
            ));
        CreateMap<EditarCompromissoResult, EditarCompromissoResponse>()
            .ConvertUsing(src => new EditarCompromissoResponse(
                src.Assunto,
                src.DataOcorrencia,
                src.HoraInicio,
                src.HoraTermino,
                src.TipoCompromisso,
                src.Local,
                src.Link,
                src.Contato
            ));
        #endregion

        #region Excluir
        CreateMap<Guid, ExcluirCompromissoCommand>()
          .ConstructUsing(src => new ExcluirCompromissoCommand(src));
        #endregion

        #region SeleçãoPorId
        CreateMap<Guid, SelecionarCompromissoPorIdQuery>()
            .ConvertUsing(src => new SelecionarCompromissoPorIdQuery(src));
        CreateMap<SelecionarCompromissoPorIdResult, SelecionarCompromissoPorIdResponse>();
        #endregion

        #region SeleçãoTodos
        CreateMap<SelecionarCompromissosRequest, SelecionarCompromissosQuery>();
        CreateMap<SelecionarCompromissosResult, SelecionarCompromissosResponse>()
            .ConvertUsing((src, dest, ctx) =>
            new SelecionarCompromissosResponse(
                src.Compromissos.Count,
                src?.Compromissos.Select(c => ctx.Mapper.Map<SelecionarCompromissosDto>(c)).ToImmutableList() ??
                ImmutableList<SelecionarCompromissosDto>.Empty
            ));
        #endregion
    }
}
