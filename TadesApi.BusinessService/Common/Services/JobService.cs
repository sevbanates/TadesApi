
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.Db.Entities;
using TadesApi.Core.Security;
using TadesApi.CoreHelper;
using TadesApi.Db.Infrastructure;

namespace TadesApi.BusinessService.CommonServices.services
{
    public class JobService : IJobService
    {
      private readonly IRepository<CmmLog> _logRepository;
        
        public JobService(IRepository<CmmLog> logRepository)
        {
            _logRepository = logRepository;
        }
        
        public void AddLog<T>(T entity, string message, SecurityModel securityModel)
        {
            //var idKey = fnc.GetEntityId<T>(entity);
            var uName = "Owner Null";
            if (securityModel != null && securityModel.UserName.IsNotInitial())
            {
                uName = securityModel.UserName;
            }
            
            var cmmLog = new CmmLog
            {
                AppCode = "TDS",
                Owner = uName,
                JsonItem = Newtonsoft.Json.JsonConvert.SerializeObject(entity),
                Date = DateTime.Now,
                TableName = typeof(T).Name,
                RecId = "idKey",
                Text = message,
                //Source=securityModel.Source
            };

            _logRepository.Insert(cmmLog);
        }

        //public JobResponse UpdateAllActions()
        //{
        //    return _redisFuncs.AddAllSecurityActions();
        //}

        //public JobResponse CheckDealerOrderItemExpire()
        //{
        //    var response = new JobResponse();
        //    var recs = _db.Query<DealerOrderItem>()
        //                  .Where(x => x.Status.EQ(Lsts.APSTAT._Aktif) && x.EndDate.LT(_cmmBusinessService.GetDate()))
        //                  .BindRelation()
        //                  .ToList();

        //    response.Comments.Add($"{recs.Count()} Adet Tarihi Geçen İş Ortağı Kontör Sipariş Kalemi Bulundu.");

        //    foreach (var rec in recs)
        //    {
        //        response.Comments.Add($"{rec.ParId} Nolu İş Ortağı Kontör Sipariş, {rec.ItemId}.Kalem Expire Odu ");

        //        rec.Status = Lsts.APSTAT._Pasif;
        //        _db.Update(rec);

        //        if (rec.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor))
        //        {
        //            var newSpend = MakeExpireDealerSpend(rec);
        //            if (rec.Remaining > 0)
        //            {
        //                _db.Insert(newSpend);
        //            }
                    

        //            _dealerCreditMovementService.CreditMovFuncs(newSpend.DealerShipId, Lsts.CREDIT_MOVEMENT._Expire, newSpend.Quantity, newSpend.OrderItemId.ToString(), newSpend.Descr, newSpend.Date);
        //        }

        //        rec.Remaining = 0;
        //        _db.Update(rec);
        //    }

        //    response.SetEntryData();
        //    return response;
        //}

        //public JobResponse CheckPurchaseOrderItemExpire()
        //{
        //    var response = new JobResponse();
        //    var recs = _db.Query<PurchaseOrderItem>()
        //                  .Where(x => x.Status.EQ(Lsts.APSTAT._Aktif) && x.EndDate.LT(_cmmBusinessService.GetDate()))
        //                  .BindRelation()
        //                  .ToList();

        //    response.Comments.Add($"{recs.Count()} Adet Tarihi Geçen Müşteri Kontör Sipariş Kalemi Bulundu.");

        //    foreach (var rec in recs)
        //    {
        //        response.Comments.Add($"{rec.ParId} Nolu Müşteri Kontör Sipariş, {rec.ItemId}.Kalem Expire Odu ");

        //        rec.Status = Lsts.APSTAT._Pasif;
        //        _db.Update(rec);

        //        if (rec.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor))
        //        {
        //            var newSpend = MakeExpireCustomerSpend(rec);
        //            if (rec.Remaining > 0)
        //            {
        //                _db.Insert(newSpend);
        //            }

        //            _creditMovementService.CreditMovFuncs(newSpend.CustomerId, newSpend.DealerShipId, Lsts.CREDIT_MOVEMENT._Expire, newSpend.Quantity, newSpend.OrderItemId.ToString(), newSpend.Descr, newSpend.Date);
        //        }

        //        rec.Remaining = 0;
        //        _db.Update(rec);
        //    }

        //    response.SetEntryData();
        //    return response;
        //}
        //public JobResponse ExecuteCalculateDealerSpend()
        //{
        //    var response = new JobResponse();

        //    var movRecs = _db.Query<DealerSpend>()
        //                     .BindRelation()
        //                     .Where(x => x.IsProcess == Lsts.POI_PROCESS._Islenecek
        //                              && x.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor))
        //                     .ToList();

        //    response.Comments.Add($"{movRecs.Count()} İş Ortağı Kontör Sipariş Bulundu.");

        //    foreach (var movRec in movRecs)
        //    {
        //        ExecuteCalculateDealerSpend_Each(movRec);
        //    }

        //    response.SetEntryData();
        //    return response;
        //}
        //public JobResponse ExecuteCalculateCustomerSpend()
        //{
        //     var response = new JobResponse();

        //    var movRecs = _db.Query<CustomerSpend>()
        //                     .BindRelation()
        //                     .Where(x => x.IsProcess == Lsts.CS_PROCESS._Islenecek
        //                              && x.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor))
        //                     .ToList();

        //    response.Comments.Add($"{movRecs.Count()}  Müşteri Kontör Sipariş Bulundu.");

        //    foreach (var movRec in movRecs)
        //    {
        //        var dealer = _db.Query<DealerShip>().BindRelation().GetFirst(x => x.Id == movRec.DealerShipId);
        //        if (dealer.SovosIsDecrease)
        //        {
        //            ExecuteCalculateCustomerSpend_Each2(movRec);
        //        }
        //        else
        //        {
        //            ExecuteCalculateCustomerSpend_Each(movRec);

        //        }

        //    }

        //    response.SetEntryData();
        //    return response;

        //}

        //public JobResponse TwoMonthsRemainingReport()
        //{
        //    var response = new JobResponse();
        //    var begDate = DateTime.Now;
        //    var endDate = DateTime.Now.AddMonths(2);
        //    var recs = _db.Query<V_PurchaseOrderItem>().Where(x => x.EndDate >= begDate && x.EndDate <= endDate).ToList();

        //    response.Comments.Add($"{recs.Count()} Sipariş Verisi Bulundu.");

        //    foreach (var rec in recs)
        //    {
        //        response.Comments.Add($"{rec.OrderNo} Nolu Siparişin Sözleşme Bitiş Tarihi Yaklaştı.");
        //        _mailService.TwoMonthsRemainingReportMail(rec);

        //    }

        //    response.SetEntryData();
        //    return response;
        //}

        //public JobResponse PercentUsageReport()
        //{
        //    var response = new JobResponse();
        //    var recs = _db.Query<V_PurchaseOrderItem>().BindRelation().Where(x => x.RemainingDiv <= 20).ToList();

        //    response.Comments.Add($"{recs.Count()} Sipariş Verisi Bulundu.");

        //    foreach (var rec in recs)
        //    {
        //        response.Comments.Add($"{rec.OrderNo} Nolu Siparişin Kontör Kullanımı %{(100-rec.RemainingDiv).ToString("0.00")} oldu.");
        //        _mailService.PercentUsageReportMail(rec);

        //    }

        //    response.SetEntryData();
        //    return response;
        //}
        //Private Function
        //public DealerSpend MakeExpireDealerSpend(DealerOrderItem item)
        //{
        //    var rec = new DealerSpend();
        //    rec.Id = 0;
        //    rec.CustomerId = 0;
        //    rec.ProductGroup = Lsts.PRODUCGROUP._Kontor;
        //    rec.Quantity = item.Remaining;
        //    rec.DealerShipId = item.ParDealerShipId;
        //    rec.IsProcess = Lsts.CS_PROCESS._Islendi;
        //    rec.Descr = item.XOrderNo + "-Kontör Zaman Aşımı";
        //    rec.RefNo = item.XOrderNo;
        //    rec.OrderItemId = item.ItemId;
        //    rec.OrderParId = item.ParId;
        //    rec.PurchaseItemId = 0;
        //    rec.PurchaseParId = 0;
        //    rec.DealerShipSubId =0;
        //    rec.Month = DateTime.Now.Month;
        //    rec.Year = DateTime.Now.Year;
        //    rec.Cmpon = DateTime.Now;
        //    rec.Date = DateTime.Now;
        //    return rec;
        //}
        //public CustomerSpend MakeExpireCustomerSpend(PurchaseOrderItem item)
        //{
        //    CustomerSpend rec = new CustomerSpend();
        //    rec.Id = 0;
        //    rec.CustomerId = item.ParCustomerId;
        //    rec.ProductGroup = Lsts.PRODUCGROUP._Kontor;
        //    rec.Quantity = item.Remaining;
        //    rec.DealerShipId = item.ParDealerShipId;
        //    rec.IsProcess = Lsts.CS_PROCESS._Islendi;
        //    rec.Descr = item.XOrderNo + "Kontör Zaman Aşımı";
        //    rec.RefNo = item.XOrderNo;
        //    rec.OrderItemId = item.ItemId;
        //    rec.OrderParId = item.ParId;
        //    rec.Month = DateTime.Now.Month;
        //    rec.Year = DateTime.Now.Year;
        //    rec.Cmpon = DateTime.Now;
        //    rec.Date = DateTime.Now;
        //    return rec;
        //}

        //private string ExecuteCalculateDealerSpend_Each(DealerSpend dealerSpend)
        //{
        //    var respList = AdJustSpendToOrder(dealerSpend);

        //    foreach (var resp in respList)
        //    {
        //        var spend = MakeAndAdjustSpend(dealerSpend, resp);

        //        if (spend.Id > 0)
        //            _db.Update(spend);
        //        else
        //            _db.Insert(spend);

        //        CheckDealerOrderStatus(resp.ParId, resp.ItemId);
        //    }
        //    return "";
        //}

        //private string ExecuteCalculateCustomerSpend_Each(CustomerSpend customerSpend)
        //{
        //    var respList = AdJustSpendToOrder(customerSpend);

        //    foreach (var resp in respList)
        //    {
        //        var spend = MakeAndAdjustSpend(customerSpend, resp);

        //        if (spend.Id > 0)
        //            _db.Update(spend);
        //        else
        //            _db.Insert(spend);

        //        _creditMovementService.CreditMovFuncs(customerSpend.CustomerId, customerSpend.DealerShipId, Lsts.CREDIT_MOVEMENT._GunlukHarcama, spend.Quantity, customerSpend.Id.ToString(), spend.Descr, customerSpend.Date);
        //        CheckPurchaseOrderStatus(resp.ParId, resp.ItemId);
        //    }
        //    return "";
        //}
        //private string ExecuteCalculateCustomerSpend_Each2(CustomerSpend customerSpend)
        //{
        //    var respList = AdJustSpendToOrder(customerSpend);

        //    foreach (var resp in respList)
        //    {
        //        var dealerSpend = new DealerSpend
        //        {
        //            Cmpon = DateTime.Now,
        //            Year = DateTime.Now.Year,
        //            Month = DateTime.Now.Month,
        //            IsProcess = Lsts.CS_PROCESS._Islendi,
        //            CustomerId = customerSpend.CustomerId,
        //            DealerShipId = customerSpend.DealerShipId,
        //            Date = DateTime.Now,
        //            Quantity = resp.Quantity,
        //            Descr = resp.Descr,
        //            OrderParId = resp.ParId,
        //            OrderItemId = resp.ItemId,
        //            ProductGroup = customerSpend.ProductGroup,
        //            RefNo = "Sözleşmesi olmayan müşteri harcaması",
        //            PurchaseParId = 0,
        //            PurchaseItemId = 0,

        //        };
        //        var spend = MakeAndAdjustSpend(dealerSpend, resp);
        //        var cSpend = MakeAndAdjustSpend(customerSpend, resp);

        //        if (spend.Id > 0)
        //            _db.Update(spend);
        //        else
        //            _db.Insert(spend);

        //        if (cSpend.Id > 0)
        //            _db.Update(cSpend);
        //        else
        //            _db.Insert(cSpend);

        //        _dealerCreditMovementService.CreditMovFuncs(customerSpend.DealerShipId,Lsts.CREDIT_MOVEMENT._GunlukHarcama, spend.Quantity, customerSpend.Id.ToString(), spend.Descr, customerSpend.Date);
        //        CheckDealerOrderStatus(resp.ParId, resp.ItemId);
        //    }
        //    return "";
        //}



        //private List<AvailableData> AdJustSpendToOrder(DealerSpend dealerSpend)
        //{
        //    var resp = new AvailableData();

        //    List<AvailableData> availableData2s = new List<AvailableData>();

        //    var orderItems = _db.Query<DealerOrderItem>()
        //                        .BindRelation()
        //                        .Where(x => x.Remaining > 0
        //                                && x.Status.EQ(Lsts.APSTAT._Aktif)
        //                                && x.XParStatus.EQ(Lsts.PORDER_STATUS._Tamamlandi)
        //                                && x.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor)
        //                                && x.ParDealerShipId.EQ(dealerSpend.DealerShipId)
        //                                && x.EndDate.GE(DateTime.Now.Date)
        //                                )
        //                        .OrderBy(x => x.EndDate)
        //                        .ToList();

        //    if (!orderItems.Any())
        //    {
        //        //TODO RAMAZAN Bayiler eksi bakiye harcaması yapamayacak ondan dolayı burada hata forlatmak mantıklı olur.
        //        availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = dealerSpend.Quantity, Descr = "Eksi Bakiye Harcama", Isnew = false });
        //        return availableData2s;
        //    }

        //    var orderItem = orderItems[0];

        //    //var totalQty = orderItem.Quantity - orderItem.Remaining;

        //    var availableQty = orderItem.Remaining;
        //    var remainingQuantity = dealerSpend.Quantity;

        //    if (availableQty > dealerSpend.Quantity)
        //    {
        //        availableQty = dealerSpend.Quantity;
        //        remainingQuantity = 0;
        //    }
        //    else
        //    {
        //        remainingQuantity = (dealerSpend.Quantity - availableQty);
        //    }

        //    availableData2s.Add(new AvailableData { ParId = orderItem.ParId, ItemId = orderItem.ItemId, Quantity = availableQty, Descr = "Günlük Harcama", Isnew = false });

        //    if (remainingQuantity <= 0)
        //        return availableData2s;

        //    var respList = AdJustSpendToOrder_Each(orderItems, remainingQuantity);

        //    availableData2s.AddRange(respList);
        //    return availableData2s;
        //}
        //private List<AvailableData> AdJustSpendToOrder(CustomerSpend customerSpend)
        //{
        //    var resp = new AvailableData();

        //    List<AvailableData> availableData2s = new List<AvailableData>();

            
        //    var dealerCheck = _db.Query<DealerShip>().BindRelation().GetFirst(x => x.Id == customerSpend.DealerShipId);
        //    if (dealerCheck.SovosIsDecrease)
        //    {
        //       var orderItems = _db.Query<DealerOrderItem>()
        //            .BindRelation()
        //            .Where(x => x.Remaining > 0
        //                        && x.Status.EQ(Lsts.APSTAT._Aktif)
        //                        && x.XParStatus.EQ(Lsts.PORDER_STATUS._Tamamlandi)
        //                        && x.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor)
        //                        && x.ParDealerShipId.EQ(customerSpend.DealerShipId)
        //                        && x.EndDate.GE(DateTime.Now.Date)
        //            )
        //            .OrderBy(x => x.EndDate)
        //            .ToList();

        //        if (!orderItems.Any())
        //        {
        //            availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = customerSpend.Quantity, Descr = "Eksi Bakiye Harcama", Isnew = false });
        //            return availableData2s;
        //        }

        //        var orderItem = orderItems[0];

        //        //var totalQty = _db.Query<CustomerSpend>()
        //        //                  .Where(x => x.OrderItemId.EQ(orderItem.ItemId)
        //        //                           && x.OrderParId.EQ(orderItem.ParId))
        //        //                  .CalculateSum(x => x.Quantity);

        //        //var availableQty = orderItem.Quantity - totalQty;
        //        var availableQty = orderItem.Remaining;
        //        var remainingQuantity = customerSpend.Quantity;

        //        if (availableQty > customerSpend.Quantity)
        //        {
        //            availableQty = customerSpend.Quantity;
        //            remainingQuantity = 0;
        //        }
        //        else
        //        {
        //            remainingQuantity = (customerSpend.Quantity - availableQty);
        //        }

        //        availableData2s.Add(new AvailableData { ParId = orderItem.ParId, ItemId = orderItem.ItemId, Quantity = availableQty, Descr = "Müşteri Günlük Harcama", Isnew = false });

        //        if (remainingQuantity <= 0)
        //            return availableData2s;


        //        var respList = AdJustSpendToOrder_Each(orderItems, remainingQuantity);

        //        availableData2s.AddRange(respList);
        //        return availableData2s;
        //    }
        //    else
        //    {
        //        var orderItems =  _db.Query<PurchaseOrderItem>()
        //                        .BindRelation()
        //                        .Where(x => x.Remaining > 0
        //                                && x.Status.EQ(Lsts.APSTAT._Aktif)
        //                                && x.XParStatus.EQ(Lsts.PORDER_STATUS._Tamamlandi)
        //                                && x.ProductGroup.EQ(Lsts.PRODUCGROUP._Kontor)
        //                                && x.ParCustomerId.EQ(customerSpend.CustomerId)
        //                                && x.EndDate.GE(DateTime.Now.Date))
        //                        .OrderBy(x => x.EndDate)
        //                        .ToList();

        //        if (!orderItems.Any())
        //        {
        //            availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = customerSpend.Quantity, Descr = "Eksi Bakiye Harcama", Isnew = false });
        //            return availableData2s;
        //        }

        //        var orderItem = orderItems[0];

        //        //var totalQty = _db.Query<CustomerSpend>()
        //        //                  .Where(x => x.OrderItemId.EQ(orderItem.ItemId)
        //        //                           && x.OrderParId.EQ(orderItem.ParId))
        //        //                  .CalculateSum(x => x.Quantity);

        //        //var availableQty = orderItem.Quantity - totalQty;
        //        var availableQty = orderItem.Remaining;
        //        var remainingQuantity = customerSpend.Quantity;

        //        if (availableQty > customerSpend.Quantity)
        //        {
        //            availableQty = customerSpend.Quantity;
        //            remainingQuantity = 0;
        //        }
        //        else
        //        {
        //            remainingQuantity = (customerSpend.Quantity - availableQty);
        //        }

        //        availableData2s.Add(new AvailableData { ParId = orderItem.ParId, ItemId = orderItem.ItemId, Quantity = availableQty, Descr = "Günlük Harcama", Isnew = false });

        //        if (remainingQuantity <= 0)
        //            return availableData2s;

        //        var respList = AdJustSpendToOrder_Each(orderItems, remainingQuantity);

        //        availableData2s.AddRange(respList);
        //        return availableData2s;
        //    }
        //}
        //private List<AvailableData> AdJustSpendToOrder_Each(List<DealerOrderItem> orderItems, decimal remainingTotal)
        //{
        //    List<AvailableData> availableData2s = new List<AvailableData>();
        //    var remaining = remainingTotal;

        //    if (remainingTotal.IsInitial())
        //        return availableData2s;

        //    if (!orderItems.Any())
        //        availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = remaining, Descr = "Eksi Bakiye Harcama", Isnew = true });

        //    int i = 0;

        //    foreach (var item in orderItems)
        //    {
        //        i++;
        //        if (i == 1)
        //            continue;
        //        var procQty = 0.00M;

        //        if (item.Quantity < remaining)
        //        {
        //            procQty = item.Quantity;
        //            remaining = remaining - item.Quantity;
        //        }
        //        else
        //        {
        //            procQty = remaining;
        //            remaining = 0;
        //        }

        //        availableData2s.Add(new AvailableData { ParId = item.ParId, ItemId = item.ItemId, Quantity = procQty, Descr = $"Devreden Günlük Harcama {i}", Isnew = true });

        //        //    remaining = remaining - procQty;
        //        if (remaining <= 0)
        //            break;
        //    }

        //    if (!availableData2s.Any())
        //    {
        //        availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = remainingTotal, Descr = "Eksi Bakiye Harcama 2", Isnew = true });
        //    }

        //    return availableData2s;
        //}
        //private List<AvailableData> AdJustSpendToOrder_Each(List<PurchaseOrderItem> orderItems, decimal remainingTotal)
        //{
        //    List<AvailableData> availableData2s = new List<AvailableData>();
        //    var remaining = remainingTotal;

        //    if (remainingTotal.IsInitial())
        //        return availableData2s;

        //    if (!orderItems.Any())
        //        availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = remaining, Descr = "Eksi Bakiye Harcama", Isnew = true });

        //    int i = 0;

        //    foreach (var item in orderItems)
        //    {
        //        i++;
        //        if (i == 1)
        //            continue;
        //        var procQty = 0.00M;

        //        if (item.Quantity < remaining)
        //        {
        //            procQty = item.Quantity;
        //            remaining = remaining - item.Quantity;
        //        }
        //        else
        //        {
        //            procQty = remaining;
        //            remaining = 0;
        //        }

        //        availableData2s.Add(new AvailableData { ParId = item.ParId, ItemId = item.ItemId, Quantity = procQty, Descr = $"Devreden Günlük Harcama {i}", Isnew = true });

        //        //remaining = remaining - procQty;
        //        if (remaining <= 0)
        //            break;
        //    }

        //    if (!availableData2s.Any())
        //    {
        //        availableData2s.Add(new AvailableData { ParId = 0, ItemId = 0, Quantity = remainingTotal, Descr = "Eksi Bakiye Harcama 2", Isnew = true });
        //    }

        //    return availableData2s;
        //}
        //private void CheckDealerOrderStatus(long parId, long itemId)
        //{
        //    if (parId == 0)
        //        return;

        //    var items = _db.Query<DealerOrderItem>().Where(x => x.ParId.EQ(parId) && x.ItemId.EQ(itemId)).BindRelation().GetFirst();
        //    var totalQty = _db.Query<DealerSpend>().Where(x => x.OrderParId.EQ(items.ParId) && x.OrderItemId.EQ(items.ItemId)).CalculateSum(x => x.Quantity);
        //    var diff = items.Quantity - totalQty;

        //    if (diff > 0)
        //    {
        //        items.Remaining = diff;
        //    }
        //    else
        //    {
        //        items.Remaining = 0;
        //    }
        //    _db.Update(items);
        //}
        //private void CheckPurchaseOrderStatus(long parId, long itemId)
        //{
        //    if (parId == 0)
        //        return;

        //    var items = _db.Query<PurchaseOrderItem>().Where(x => x.ItemId.EQ(itemId) && x.ParId.EQ(parId)).BindRelation().GetFirst();
        //    var totalQty = _db.Query<CustomerSpend>().Where(x => x.OrderItemId.EQ(items.ItemId) && x.OrderParId.EQ(items.ParId)).CalculateSum(x => x.Quantity);
        //    var diff = items.Quantity - totalQty;

        //    if (diff > 0)
        //    {
        //        items.Remaining = diff;
        //    }
        //    else
        //    {
        //        items.Remaining = 0;
        //    }
        //    _db.Update(items);
        //}


        //private DealerSpend MakeAndAdjustSpend(DealerSpend spend, AvailableData availableData)
        //{
        //    var rec = spend.Clone();
        //    if (availableData.Isnew)
        //    {
        //        rec.Id = 0;
        //        rec.CustomerId = spend.CustomerId;
        //    }
        //    rec.Descr = availableData.Descr;
        //    rec.OrderItemId = availableData.ItemId;
        //    rec.OrderParId = availableData.ParId;
        //    rec.Quantity = availableData.Quantity;
        //    rec.IsProcess = Lsts.CS_PROCESS._Islendi;
        //    rec.Month = DateTime.Now.Month;
        //    rec.Year = DateTime.Now.Year;
        //    rec.Cmpon = DateTime.Now;
        //    return rec;
        //}

        //private CustomerSpend MakeAndAdjustSpend(CustomerSpend spend, AvailableData availableData)
        //{
        //    var rec = spend.Clone();
        //    if (availableData.Isnew)
        //    {
        //        rec.Id = 0;
        //        rec.CustomerId = spend.CustomerId;
        //    }
        //    rec.Descr = availableData.Descr;
        //    rec.OrderItemId = availableData.ItemId;
        //    rec.OrderParId = availableData.ParId;
        //    rec.Quantity = availableData.Quantity;
        //    rec.IsProcess = Lsts.CS_PROCESS._Islendi;
        //    rec.Month = DateTime.Now.Month;
        //    rec.Year = DateTime.Now.Year;
        //    rec.Cmpon = DateTime.Now;
        //    return rec;
        //}


        public class AvailableData
        {
            public bool Isnew { get; set; } = false;
            public long ItemId { get; set; }
            public long ParId { get; set; }
            public string Descr { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
