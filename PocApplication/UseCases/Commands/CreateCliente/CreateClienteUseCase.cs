using PocDomain.Aggregate.Cliente;
public class CreateClienteUseCase: ICreateClienteUseCase
{  
     private readonly IClienteRepository _repository;

      public CreateClienteUseCase(IClienteRepository repository){
        _repository = repository;
      }


     public async Task Execute(CreateClienteDto Value){

          Cliente cliente = Cliente.Create(Value.Nome);
          _repository.Add(cliente);
          await _repository.SaveChangesAsync();
     }

}