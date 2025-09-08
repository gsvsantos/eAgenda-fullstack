using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloAutenticacao.Commands;
using eAgenda.WebAPI.Models.ModuloAutenticacao;

namespace eAgenda.WebAPI.AutoMapper;

public class AutenticacaoModelsMappingProfile : Profile
{
    public AutenticacaoModelsMappingProfile()
    {
        CreateMap<RegistrarUsuarioRequest, RegistrarUsuarioCommand>();
        CreateMap<AutenticarUsuarioRequest, AutenticarUsuarioCommand>();
    }
}
