using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.ModuloCompromisso;

namespace eAgenda.Core.Aplicacao.AutoMapper;
public class CompromissoMappingProfile : Profile
{
    public CompromissoMappingProfile()
    {
        #region Cadastrar
        CreateMap<CadastrarCompromissoCommand, Compromisso>();
        CreateMap<Compromisso, CadastrarCompromissoResult>();
        #endregion
    }
}
