using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Grpc.Core.Metadata;
using System.Diagnostics;

namespace EJMSubscribeFunction.Services
{
    public class SubscribeServices
    {
        private readonly DataContext _context;
        public SubscribeServices(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateSubscriberAsync(SubscriberEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    await _context.Subscribers.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateSubscriberFunction::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> CheckIfExsitsAsync(SubscriberEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    var existingEntity = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == entity.Id);

                    if (existingEntity != null && existingEntity.Id != null)
                    {
                        return new OkObjectResult(existingEntity);
                    }

                    var existingEmail = await _context.Subscribers.FirstOrDefaultAsync(y => y.Email == entity.Email);

                    if (existingEmail != null && existingEmail.Email != null)
                    {
                        return new ConflictResult();
                    }

                    var existingUser = await _context.Subscribers.FirstOrDefaultAsync(x => x.UserId == entity.UserId);

                    if(existingUser != null && existingUser.UserId != null)
                    {
                        return new OkObjectResult(existingUser);
                    }

                    return new NotFoundResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CheckExsitAndUpdateAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> CheckIfUserExsitsAsync(SubscriberEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    var existingUser = await _context.Subscribers.FirstOrDefaultAsync(x => x.UserId == entity.UserId);

                    if (existingUser != null)
                    {
                        return new OkObjectResult(existingUser);
                    }

                    return new NotFoundResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CheckExsitAndUpdateAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> UpdateAsync(SubscriberEntity entity, SubscriberEntity existingEntity)
        {
            try
            {
                if (entity != null)
                {
                        existingEntity.Email = entity.Email;
                        _context.Entry(existingEntity).State = EntityState.Modified;
                        _context.SaveChanges();
                        return new OkResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CheckExsitAndUpdateAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> DeleteSubscriberAsync(SubscriberEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    _context.Subscribers.Remove(entity);
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteSubscriberAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetASubscriberAsync(string Userid)
        {
            try
            {
                if (Userid != null)
                {
                    var entityToFind = await _context.Subscribers.FirstOrDefaultAsync(x => x.UserId == Userid);
                    
                    if(entityToFind != null)
                    {
                        return new OkObjectResult(entityToFind);
                    }
                }

                return new BadRequestObjectResult( new { Status = 400, Message = "No subscriber found..." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetASubscriberAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetAllSubscribersAsync()
        {
            try
            {
                List<SubscriberEntity> subscribers = await _context.Subscribers.ToListAsync();

                if(subscribers.Any())
                {
                    return new OkObjectResult(subscribers);
                }

                return new NotFoundObjectResult(new { Status = 404, Message = "No subscribers found..." });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAllSubscribersAsync::" + ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
