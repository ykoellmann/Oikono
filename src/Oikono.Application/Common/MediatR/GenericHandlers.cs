using MapsterMapper;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Models;
using ErrorOr;

namespace Oikono.Application.Common.MediatR
{
    public class GetListQueryHandler<TIRepository, TEntity, TId, TResult>
        : IQueryHandler<GetListQuery<TEntity, TId, TResult>, List<TResult>>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TIRepository : IRepository<TEntity, TId>
    {
        private readonly TIRepository _repo;
        private readonly IMapper _mapper;

        public GetListQueryHandler(TIRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<List<TResult>>> Handle(GetListQuery<TEntity, TId, TResult> request,
            CancellationToken ct)
        {
            var entities = await _repo.GetListAsync(ct);
            return _mapper.Map<List<TResult>>(entities);
        }
    }

    public class GetByIdQueryHandler<TIRepository, TEntity, TId, TResult>
        : IQueryHandler<GetByIdQuery<TEntity, TId, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TIRepository : IRepository<TEntity, TId>
    {
        private readonly TIRepository _repo;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(TIRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<TResult>> Handle(GetByIdQuery<TEntity, TId, TResult> request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return _mapper.Map<TResult>(entity);
        }
    }
    
    public class CreateCommandHandler<TIRepository, TEntity, TId, TRequest, TResult>
        : ICommandHandler<CreateCommand<TEntity, TId, TRequest, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TIRepository : IRepository<TEntity, TId>
    {
        private readonly TIRepository _repo;
        private readonly IMapper _mapper;

        public CreateCommandHandler(TIRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<TResult>> Handle(
            CreateCommand<TEntity, TId, TRequest, TResult> request,
            CancellationToken ct)
        {
            var entity = _mapper.Map<TEntity>(request.Request);
            var created = await _repo.AddAsync(entity, request.UserId, ct);
            return _mapper.Map<TResult>(created);
        }
    }
    
    public class UpdateCommandHandler<TIRepository, TEntity, TId, TRequest, TResult>
        : ICommandHandler<UpdateCommand<TEntity, TId, TRequest, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TIRepository : IRepository<TEntity, TId>
    {
        private readonly TIRepository _repo;
        private readonly IMapper _mapper;

        public UpdateCommandHandler(TIRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<TResult>> Handle(
            UpdateCommand<TEntity, TId, TRequest, TResult> request,
            CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            entity = _mapper.Map(request, entity);
            
            var updated = await _repo.UpdateAsync(entity, request.UserId, ct);
            
            return _mapper.Map<TResult>(updated);
        }
    }
    
    public class DeleteCommandHandler<TIRepository, TEntity, TId>
        : ICommandHandler<DeleteCommand<TEntity, TId>, Deleted>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TIRepository : IRepository<TEntity, TId>
    {
        private readonly TIRepository _repo;

        public DeleteCommandHandler(TIRepository repo)
        {
            _repo = repo;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteCommand<TEntity, TId> request,
            CancellationToken ct)
        {
            return await _repo.DeleteAsync(request.Id, ct);
        }
    }
}