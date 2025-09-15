using MapsterMapper;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Domain.Models;
using ErrorOr;

namespace Oikono.Application.Common.MediatR
{
    public class GetListQueryHandler<TEntity, TId, TResult>
        : IQueryHandler<GetListQuery<TEntity, TResult>, IEnumerable<TResult>>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
    {
        private readonly IRepository<TEntity, TId> _repo;
        private readonly IMapper _mapper;

        public GetListQueryHandler(IRepository<TEntity, TId> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<TResult>>> Handle(GetListQuery<TEntity, TResult> request,
            CancellationToken ct)
        {
            var entities = await _repo.GetListAsync(ct);
            return _mapper.Map<List<TResult>>(entities);
        }
    }

    public class GetByIdQueryHandler<TEntity, TId, TResult>
        : IQueryHandler<GetByIdQuery<TEntity, TId, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
    {
        private readonly IRepository<TEntity, TId> _repo;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(IRepository<TEntity, TId> repo, IMapper mapper)
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
    
    public class CreateCommandHandler<TEntity, TId, TRequest, TResult>
        : ICommandHandler<CreateCommand<TEntity, TRequest, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
    {
        private readonly IRepository<TEntity, TId> _repo;
        private readonly IMapper _mapper;

        public CreateCommandHandler(IRepository<TEntity, TId> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ErrorOr<TResult>> Handle(
            CreateCommand<TEntity, TRequest, TResult> request,
            CancellationToken ct)
        {
            var entity = _mapper.Map<TEntity>(request.Request);
            var created = await _repo.AddAsync(entity, request.UserId, ct);
            return _mapper.Map<TResult>(created);
        }
    }
    
    public class UpdateCommandHandler<TEntity, TId, TRequest, TResult>
        : ICommandHandler<UpdateCommand<TEntity, TId, TRequest, TResult>, TResult>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
    {
        private readonly IRepository<TEntity, TId> _repo;
        private readonly IMapper _mapper;

        public UpdateCommandHandler(IRepository<TEntity, TId> repo, IMapper mapper)
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
    
    public class DeleteCommandHandler<TEntity, TId>
        : ICommandHandler<DeleteCommand<TEntity, TId>, Deleted>
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
    {
        private readonly IRepository<TEntity, TId> _repo;

        public DeleteCommandHandler(IRepository<TEntity, TId> repo)
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