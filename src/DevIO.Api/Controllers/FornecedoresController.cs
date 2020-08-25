using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DevIO.Api.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                        IMapper mapper,
                                        IFornecedorService fornecedorService,
                                        INotificador notificador, 
                                        IEnderecoRepository enderecoRepository) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        
        {
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>> (await _fornecedorRepository.ObterTodos());

            return fornecedor;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterTodosPorID(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

       

        [HttpPost()]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel  fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorService.Adicionar(fornecedor);

            return CustomResponse(fornecedorViewModel);
 
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorService.Atualizar(fornecedor);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Remover(Guid id)
        {

            var fornecedor = _fornecedorRepository.ObterPorId(id);

            if (fornecedor == null) return NotFound();

            bool result = await _fornecedorService.Remover(id);

            return CustomResponse();

        }
        
        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterEnderecoPorID(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _enderecoRepository.ObterPorId(id));

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var endereco = _mapper.Map<Endereco>(enderecoViewModel);

            await _enderecoRepository.Atualizar(endereco);

            return CustomResponse(enderecoViewModel);
        }
    }
}
