using PocDomain.Aggregate.Cliente;
public class CreateClienteUseCase: ICreateClienteUseCase
{  
     private readonly IClienteRepository _repository;

      public CreateClienteUseCase(IClienteRepository repository){
        _repository = repository;
      }


     public async Task Execute(CreateClienteDto Value){

          Cliente cliente = Cliente.Create(
               new Name(Value.Nome), 
               new BirthDate(Value.Nascimento)
               );

          if(!cliente.MaiorIdade()){
            throw new Exception("O Cliente não é maior de Idade.");
          }
          
          _repository.Add(cliente);
          await _repository.SaveChangesAsync();
     }

}