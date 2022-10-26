﻿using APIProject.Data;
using APIProject.Models;
using APIProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Provider
{
    public class FoodProvider : IProvider
    {


        private readonly FoodContext fd;
        //public string SearchTerm { get; set; }
        public FoodProvider(FoodContext fd)
        {
            this.fd = fd;
        }

        public async Task<UserList> AddNewUser(UserList U)
        {
             fd.UserList.Add(U);
             await fd.SaveChangesAsync();
            return U;
           
        }
        #region
        //public Cart AddtoCart(int Qnt, int FoodId, int UserId)
        //{
        //    #region
        //    //var F = fd.Food.FirstOrDefault(i => i.FoodId == C.FoodId);
        //    //var  U= fd.UserList.FirstOrDefault(i => i.UserId == C.UserId);
        //    #endregion
        //    Cart C = new Cart();
        //    var F = fd.Food.FirstOrDefault(i => i.FoodId == FoodId);
        //    var U = fd.UserList.FirstOrDefault(i => i.UserId == UserId);

        //    C.UserId =UserId;
        //    C.FoodId = F.FoodId;
        //    C.Qnt = Qnt;
        //    fd.Add(C);
        //    fd.SaveChanges();
        //    return C;
        //}

        public async Task<Cart> AddtoCart(Cart C)
        {
            Cart T = new Cart();
            var F = fd.Food.FirstOrDefault(i => i.FoodId == C.FoodId);
            var U = fd.UserList.FirstOrDefault(i => i.UserId == C.UserId);
            T.UserId = U.UserId;
            T.FoodId = F.FoodId;
            T.Qnt = C.Qnt;
            fd.Add(T);
            await fd.SaveChangesAsync();
            return T;
        }
        #endregion

        public async Task<OrderMaster> Buy(int UserId)
        {
            return await(fd.OrderMaster.FirstOrDefaultAsync(m => m.UserId == UserId));
           
        }

        public async Task<Cart> Delete(int CartId)
        {
            return await (fd.Cart.FirstOrDefaultAsync(m => m.CartId == CartId));

        }

        public async Task<bool> DeleteConfirmed(int CartId)
        {
            //var val = ;
            fd.Cart.Remove(fd.Cart.Find(CartId));
            await fd.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmptyList(int UserId)
        {
            List<Cart> list = (from i in fd.Cart
                               where i.UserId == UserId
                               select i).ToList();
            foreach (var item in list)
            {
                var val = fd.Cart.Find(item.CartId);
                fd.Cart.Remove(val);
                fd.SaveChanges();
            }
            return true;
        }

        public async Task<List<Food>> GetAll()
        {
            //if (SearchText != "" && SearchText != null)
            //{
            //    return fd.Food.Where(P => P.FoodName.Contains(SearchText)).ToList();
            //}
            //else
            //{
                return await fd.Food.ToListAsync();
            //}

        }

        public async Task<List<Cart>> GetCartByUserId(int UserId)
        {
            return await (from i in fd.Cart.Include(x => x.Food)
                    where i.UserId == UserId
                    select i).ToListAsync();


        }

        public async Task<Food> GetFoodById(int? id)
        {
            return await fd.Food.FindAsync(id);

        }

        public async Task<UserList> Login(UserList U)
        {
            var result = await (from i in fd.UserList
                          where i.FName == U.FName && i.Password == U.Password && i.Role == U.Role
                          select i).SingleOrDefaultAsync();

            return result;

        }


        public async Task<OrderMaster> Pay(int OrderId)
        {
            var result = await fd.OrderMaster.SingleOrDefaultAsync(m => m.OrderId == OrderId);
            return result;
        }


        public async Task<bool> Pay(int OrderId, OrderMaster O)
        {
            var result = fd.OrderMaster.SingleOrDefault(m => m.OrderId == OrderId);
            result.BankName = O.BankName;
            result.CardNo = O.CardNo;
            result.CCV = O.CCV;
            await fd.SaveChangesAsync();
            return true;
        }
        public async Task<List<OrderDetails>> OrderDetails()
        {
            var C = await fd.OrderDetails.ToListAsync();
            return C;
        }

        //public void Payment(int OrderId, string Type)
        //{
        //    var result = fd.OrderMaster.SingleOrDefault(m => m.OrderId == OrderId);
        //    result.Type = Type;
        //    fd.SaveChanges();
        //}
        public async Task<OrderMaster> Payment(int OrderId, string Type)
        {
            var result = fd.OrderMaster.SingleOrDefault(m => m.OrderId == OrderId);
            result.Type = Type;
            await fd.SaveChangesAsync();
            return result;
        }

        public async Task<bool> ViewCart(int? UserId)
        {

            List<Cart> list = (from i in fd.Cart
                               where i.UserId == UserId
                               select i).ToList();
            OrderMaster orderMaster = new OrderMaster();


            orderMaster.UserId = UserId;
            fd.Add(orderMaster);
            fd.SaveChanges();


            List<OrderDetails> orderDetails = new List<OrderDetails>();
            foreach (var item in list)
            {

                var F = fd.Food.SingleOrDefault(i => i.FoodId == item.FoodId);


                OrderDetails od = new OrderDetails();
                od.OrderId = orderMaster.OrderId;
                od.FoodId = item.FoodId;
                od.Qnt = item.Qnt;
                od.Price = F.price;
                od.TotalPrice = od.Qnt * od.Price;
                fd.OrderDetails.Add(od);
                fd.SaveChanges();


            }
            orderDetails.AddRange(fd.OrderDetails);
            orderMaster.TotalPrice = orderDetails.Sum(i => i.TotalPrice);
            await fd.SaveChangesAsync();
            return true;
        }
        public async Task<Food> AddNewFood(Food food)
        {
            fd.Add(food);
            await fd.SaveChangesAsync();
            return food;
        }

        public async Task<bool> Edit(int CartId, Cart C)
        {
            fd.Cart.Update(C);
            await fd.SaveChangesAsync();
            return true;
        }

        public async Task<Cart> GetCartByCartId(int CartId)
        {
            return await (fd.Cart.FindAsync(CartId));
        }

        public async Task<bool> DeleteCart(int CartId)
        {
            Cart c = fd.Cart.Find(CartId);
            fd.Remove(c);
            await fd.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditFood(int id, Food food)
        {
            //if (id != food.FoodId)
            //{
            //    return BadRequest();
            //}

            //fd.Entry(food).State = EntityState.Modified;
             fd.Update(food);

            //try
            //{
            await fd.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!FoodExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            return true;
        }
        public async Task<bool> DeleteFood(int FoodId)
        {
            Food c = fd.Food.Find(FoodId);
             fd.Remove(c);
            await fd.SaveChangesAsync();
            return true;
        }
        public List<UserList> UserDetails()
        {
            var C = (from i in fd.UserList
                     where i.Role == "User"
                     select i).ToList();
            return C;
        }

        //public List<Content> GetReportById(int? UserId)
        //{

        //    var O = (from i in fd.OrderMaster
        //             where i.UserId == UserId
        //             select i).ToList();
        //    List<Content> ct = new List<Content>();
        //    foreach (var k in O)
        //    {
        //        List<OrderDetails> list =
        //        (from i in fd.OrderDetails
        //         where i.OrderId == k.OrderId
        //         select i).ToList();
        //        //}
        //        //Content content = new Content();

        //        foreach (var item in list)
        //        {

        //            var T = fd.Food.SingleOrDefault(i => i.FoodId == item.FoodId);
        //            var Y = fd.OrderDetails.SingleOrDefault(i => i.FoodId == item.FoodId);
        //            var F = fd.UserList.Find(UserId);

        //            Content od = new Content();


        //            od.Email = F.Email;

        //            od.FoodName = T.FoodName;
        //            od.Image = T.Image;
        //            od.Qnt = Y.Qnt;
        //            od.Price = Y.Price;

        //            od.TotalPrice = Y.TotalPrice;
        //            fd.SaveChanges();
        //            ct.Add(od);
        //        };

        //    }

        //    fd.SaveChanges();


        //    return ct;

        //}

        public async Task<List<NewOrder>> ViewNewOrder()
        {
            List<OrderDetails> list = fd.OrderDetails.ToList();

            List<NewOrder> ct = new List<NewOrder>();

           
                foreach (var item in list)
            {

                var T = fd.Food.SingleOrDefault(i => i.FoodId == item.FoodId);
                var Y = fd.OrderDetails.SingleOrDefault(i => i.FoodId == item.FoodId);
                //var F=(from i in fd.OrderMaster
                //      where i.OrderId==item.OrderId
                //      select i.User.Email);
                var F = fd.OrderMaster.SingleOrDefault(i => i.OrderId == item.OrderId);
                var L =fd.UserList.Find(F.UserId);
                NewOrder od = new NewOrder();


                od.Email = L.Email;
                od.OrderId= (int)item.OrderId;
                od.FoodName = T.FoodName;
                od.Image = T.Image;
                od.Qnt = Y.Qnt;
                od.Price = Y.Price;
                //od.status = "Dispatch";
                od.TotalPrice = Y.TotalPrice;
                od.Status = "Dispatch";
                ct.AddRange(fd.NewOrder);
                fd.SaveChanges();

            };





            await fd.SaveChangesAsync();


            return ct;

        }
        public async Task<bool> DispatchOrder(int Id)
        {
            
            NewOrder c = fd.NewOrder.Find(Id);
            c.Status = "Order Dispatched";
            fd.Update(c);
            fd.SaveChanges();
            return true;
        }

        public async Task<NewOrder> DispatchNewOrder(int Id)
        {
            return await fd.NewOrder.FirstOrDefaultAsync(m => m.Id == Id);

        }
        public async Task<bool> EmptyOrder(int OrderId)
        {
            List<OrderDetails> list = (from i in fd.OrderDetails
                                       where i.OrderId == OrderId
                                       select i).ToList();
            List<OrderMaster> list2 = (from i in fd.OrderMaster
                                       where i.OrderId == OrderId
                                       select i).ToList();
            foreach (var item in list)
            {
                var val = (from i in fd.OrderDetails
                           where i.OrderId == item.OrderId
                           select i).ToList();
                foreach (var i in val)
                {
                    fd.OrderDetails.Remove(i);
                }
                fd.SaveChanges();
            }
            foreach (var item in list2)
            {
                var val = fd.OrderMaster.Find(item.OrderId);
                fd.OrderMaster.Remove(val);
                await fd.SaveChangesAsync();
            }
            return true;
        }

    }
}
    












