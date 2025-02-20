public interface ICreateClienteUseCase{
  
  Task<Result<Guid>> Execute(CreateClienteDto Value); 

}