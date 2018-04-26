using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ExmoAPI.Authenticated_API.Classes;
using ExmoAPI.Generic;
using ExmoAPI.Public_API.Classes;

#pragma warning disable 1587

namespace ExmoTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            /***********************/
            /********Example********/
            /***********************/



            /*Public API*/

            string apiName;
            var apiKey = new ExmoApi("K", "S");
            string tradeCouples = "LTC_RUB";
            int? limit = 10;

            ///<summary>trades
            /// <remarks>Список сделок по валютной паре</remarks>
            /// <param name="tradeCouples">одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)</param>
            /// <param name="limit">кол-во отображаемых позиций (по умолчанию 100, максимум 1000)</param>
            ///<returns>ResultList type=CTrade ></returns>
            /// </summary>
            IHelperPublicAPI<CTrades> testTradesApi=new CHelperPublicAPI<CTrades>();
            await testTradesApi.GetResultAsync("trades", null, tradeCouples, limit);
            //testTradesApiResult.Wait();
            Console.WriteLine($"Список сделок по валютной паре {tradeCouples}:");
            foreach (var tmp in testTradesApi.ResultList)
            {
                Console.WriteLine($"{tmp.TradeId} {tmp.Type} {tmp.Price} {tmp.Quantity} {tmp.Amount} {(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(tmp.Date)}");
            }
            
            ///<summary>order_book
            /// <remarks>Книга ордеров по валютной паре</remarks>
            /// <param name="tradeCouples">одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)</param>
            /// <param name="limit">кол-во отображаемых позиций (по умолчанию 100, максимум 1000)</param>
            ///<returns>ResultMethod type=COrderBook></returns>
            /// </summary>
            IHelperPublicAPI<COrderBook> testOrderBookApi=new CHelperPublicAPI<COrderBook>();
            await testOrderBookApi.GetResultAsync("order_book", null, tradeCouples, limit);
            //testOrderBookApiTask.Wait();
            Console.WriteLine("\nКнига ордеров по валютной паре:");
            Console.WriteLine($"{testOrderBookApi.ResultMetod.AskQuantity} {testOrderBookApi.ResultMetod.AskAmount} {testOrderBookApi.ResultMetod.AskTop}, {testOrderBookApi.ResultMetod.BidQuantity}, " +
                              $"{testOrderBookApi.ResultMetod.BidAmount}, {testOrderBookApi.ResultMetod.BidTop}");
            Console.WriteLine("\nСписок ордеров на покупку:");
            foreach (var i in testOrderBookApi.ResultMetod.Ask)
            {
                Console.WriteLine($"{i[0]}, {i[1]}, {i[2]}");
            }
            Console.WriteLine("\nСписок ордеров на продажу:");
            foreach (var i in testOrderBookApi.ResultMetod.Bid)
            {
                Console.WriteLine($"{i[0]}, {i[1]}, {i[2]}");
            }
            
            ///<summary>ticker
            /// <remarks>Cтатистика цен и объемов торгов по валютным парам</remarks>
            ///<returns>ResultMethod type=CTicker</returns>
            /// </summary>
            IHelperPublicAPI<CTicker> testTickerApi=new CHelperPublicAPI<CTicker>();
            await testTickerApi.GetResultAsync("ticker", null, tradeCouples);
            //testTickerApiTask.Wait();
            Console.WriteLine($"\nCтатистика цен и объемов торгов по валютной паре {tradeCouples}:");
            Console.WriteLine($"{testTickerApi.ResultMetod.High}, {testTickerApi.ResultMetod.Low}, {testTickerApi.ResultMetod.Avg}, " +
                              $"{testTickerApi.ResultMetod.Vol}, {testTickerApi.ResultMetod.VolCurr}, {testTickerApi.ResultMetod.LastTrade}, " +
                              $"{testTickerApi.ResultMetod.BuyPrice}, {testTickerApi.ResultMetod.SellPrice}, " +
                              $"{(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(testTickerApi.ResultMetod.Updated)}");

            ///<summary>pair_settings
            /// <remarks>Настройки валютных пар</remarks>
            /// <param name="tradeCouples">одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)</param>
            ///<returns>ResultMethod type=CPairSettings></returns>
            /// </summary>
            IHelperPublicAPI<CPairSettings> testPairSettingApi=new CHelperPublicAPI<CPairSettings>();
            await testPairSettingApi.GetResultAsync("pair_settings", null, tradeCouples);
            //testPairSettingApiTask.Wait();
            Console.WriteLine($"\nНастройки валютной пары {tradeCouples}:");
            Console.WriteLine($"{testPairSettingApi.ResultMetod.MinQuantity}, {testPairSettingApi.ResultMetod.MaxQuantity} " +
                              $"{testPairSettingApi.ResultMetod.MinPrice}, {testPairSettingApi.ResultMetod.MaxPrice}, " +
                              $"{testPairSettingApi.ResultMetod.MinAmount}, {testPairSettingApi.ResultMetod.MaxAmount}");

            ///<summary>currency
            /// <remarks>Cписок валют биржи</remarks>
            ///<returns>CurrencyList type=IList<string>></returns>
            /// </summary>
            await CCurrency.GetCurrencyAsync(null);
            //currencyTask.Wait();
            Console.WriteLine("\nCписок валют биржи:");
            foreach (var i in CCurrency.CurrencyList)
            {
                Console.WriteLine(i);
            }

            //<summary>currencyPair
            /// <remarks>Cписок валютных пар (не входит в ExmoApi)</remarks>
            ///<returns>CurrencyPairList type=IList<string>></returns>
            /// </summary>
            await CCurrency.GetCurrencyPairListAsync(null);
            //currencyPairTaskTask.Wait();
            Console.WriteLine("\nCписок валютных пар биржи:");
            foreach (var i in CCurrency.CurrencyPairList)
            {
                Console.WriteLine(i);
            }


            /*Authenticated  API*/

            apiKey = new ExmoApi("K", "S-");
            tradeCouples = "BTC_USD";
            limit = 10;
            decimal quantity = 0.001M; //BTC
            decimal price = 20000M; //Для продажи BTC
            string type = "sell";

            
            ///<summary>user_info
            /// <remarks>Получение информации об аккаунте пользователя</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            ///<returns>ResultMethod type=CUserInfo ></returns>
            /// </summary>
            IHelperAuthAPI<CUserInfo> testUserInfoApi=new CHelperAuthAPI<CUserInfo>();
            await testUserInfoApi.GetResultAsync("user_info", apiKey);
            Console.WriteLine("\nИнформация об аккаунте пользователя");
            Console.WriteLine($"{testUserInfoApi.ResultMetod.Uid} {testUserInfoApi.ResultMetod.ServerDate}");
            //Проход по свойствам класса Balances
            Console.WriteLine("\nБаланс:");
            foreach (var propInfo in testUserInfoApi.ResultMetod.Balances.GetType().GetProperties())
                Console.WriteLine($"{propInfo.Name}: {propInfo.GetValue(testUserInfoApi.ResultMetod.Balances, null)}");
            //Проход по свойствам класса Reserved
            Console.WriteLine($"\nВ ордерах:");
            foreach (var propInfo in testUserInfoApi.ResultMetod.Reserved.GetType().GetProperties())
                Console.WriteLine($"{propInfo.Name}: {propInfo.GetValue(testUserInfoApi.ResultMetod.Reserved, null)}");
            
            ///<summary>order_create
            /// <remarks>Создание ордера</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            /// <param name="Dictionary">Словарь, содержащий следующие параметры:
            ///         tradeCouples - валютная пара
            ///         quantity - кол-во по ордеру
            ///         price - цена по ордеру
            ///         type - тип ордера, может принимать следующие значения:
            ///             buy - ордер на покупку
            ///             sell - ордер на продажу
            ///             market_buy - ордера на покупку по рынку 
            ///             market_sell - ордер на продажу по рынку 
            ///             market_buy_total - ордер на покупку по рынку на определенную сумму
            ///             market_sell_total - ордер на продажу по рынку на определенную сумму</param> 
            /// <param name="tradeCouples">Валютная пара</param> 
            ///<returns>ResultMethod type=COrderCreate ></returns>
            /// </summary>
            IHelperAuthAPI<COrderCreate> testOrderCreateApi=new CHelperAuthAPI<COrderCreate>();
            await testOrderCreateApi.GetResultAsync("order_create", apiKey,
                new Dictionary<string, string>()
                {
                    {"pair", tradeCouples.ToString(CultureInfo.InvariantCulture)},
                    {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                    {"price", price.ToString(CultureInfo.InvariantCulture)},
                    {"type", type}

                }, tradeCouples);
            Console.WriteLine("\nСоздание ордера:");
            if (testOrderCreateApi.ResultMetod.Result)
                Console.WriteLine($"\nОрдер создан:" +
                                  $"\nПара: {tradeCouples}" +  
                                  $"\norder_id: {testOrderCreateApi.ResultMetod.OrderId}" +
                                  $"\nОперация: {type}" +
                                  $"\nЦена: {price}" +
                                  $"\nКоличество: {quantity}" +
                                  $"\nСумма: {price*quantity}");
            else
            {
                Console.WriteLine($"\nОрдер не создан!!!" +
                                  $"\nОшибка: {testOrderCreateApi.ResultMetod.Error}");
            }

            var orderId = testOrderCreateApi.ResultMetod.OrderId;
            
            //<summary>order_cancel
            /// <remarks>Отмена ордера</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            /// <param name="Dictionary">Словарь, содержащий следующие параметры:
            ///         order_id -  идентификатор ордера </param> 
            ///<returns>ResultMethod type=COrderCancel ></returns>
            /// </summary>
            IHelperAuthAPI<COrderCancel> testOrderCancelApi=new CHelperAuthAPI<COrderCancel>();
            await testOrderCancelApi.GetResultAsync("order_cancel", apiKey,
                new Dictionary<string, string>()
                {
                    {"order_id", orderId.ToString(CultureInfo.InvariantCulture)}
                });
            Console.WriteLine($"\nОтмена ордера {orderId}:");
            if(testOrderCancelApi.ResultMetod.Result)
                Console.WriteLine($"\nОрдер c номером {orderId} - отменен.");
            else
            {
                Console.WriteLine($"\nОрдер не отменен!!!" +
                                  $"\nОшибка: {testOrderCancelApi.ResultMetod.Error}");
            }

            
            //<summary>user_open_orders
            /// <remarks>Отмена ордера</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            ///<returns>ResultMethod type=CUserOpenOrders ></returns>
            /// </summary>
            IHelperAuthAPI<CUserOpenOrders> testUserOpenOrdersApi=new CHelperAuthAPI<CUserOpenOrders>();
            await testUserOpenOrdersApi.GetResultAsync("user_open_orders", apiKey);
            foreach (var res in testUserOpenOrdersApi.ResultList)
            {
                Console.WriteLine($"\nСписок открытых ордеров пользователя:" +
                                  $"\nВалютная пара: {res.TradeCouples}"+
                                  $"\nOrder_id: {res.OrderId}"+
                                  $"\nСоздан: {(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(res.CreatedTime)}"+
                                  $"\nТип: {res.Type}"+
                                  $"\nЦена: {res.Price}"+
                                  $"\nКоличество: {res.Quantity}"+
                                  $"\nСумма: {res.Amount}");
            }

            //<summary>user_trades
            /// <remarks>Получение сделок пользователя</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            /// <param name="Dictionary">Словарь, содержащий следующие параметры:
            ///         tradeCouples - одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)
            ///         limit - кол-во возвращаемых сделок (по умолчанию 100, максимум 10 000)
            ///         offset - смещение от последней сделки (по умолчанию 0)</param> 
            ///<returns>ResultMethod type=CUserTrades ></returns>
            /// </summary>
            IHelperAuthAPI<CUserTrades> testUserTradesApi=new CHelperAuthAPI<CUserTrades>();
            await testUserTradesApi.GetResultAsync("user_trades", apiKey,
                new Dictionary<string, string>()
                {
                    {"pair", "ETC_USD, BTC_USD"},
                    {"limit", "100" },
                    {"offset", "0" }
                });
            Console.WriteLine("\nСделки пользователя:");
            foreach (var uT in testUserTradesApi.ResultList)
            {
                Console.WriteLine($"\nВалютная пара: {uT.TradeCouples}" +
                                  $"\ntrade_id: {uT.TradeId}" +
                                  $"\ndate : {uT.Date}"+
                                  $"\ntype : {uT.Type}" +
                                  $"\norder_id: {uT.OrderId}" +
                                  $"\nquantity : {uT.Quantity}" +
                                  $"\nprice : {uT.Price}" +
                                  $"\namount : {uT.Amount}" );
            }
            
            //<summary>user_cancelled_orders
            /// <remarks>Получение отмененных ордеров пользователя</remarks>
            /// <param name="apiKey">Идентификатор пользователя на бирже</param>
            /// <param name="Dictionary">Словарь, содержащий следующие параметры:
            ///         limit - кол-во возвращаемых сделок (по умолчанию 100, максимум 10 000)
            ///         offset - смещение от последней сделки (по умолчанию 0)</param> 
            ///<returns>ResultMethod type=CUserTrades ></returns>
            /// </summary>
            IHelperAuthAPI<CUserCancelledOrders> testUserCansOrdersApi=new CHelperAuthAPI<CUserCancelledOrders>();
            await testUserCansOrdersApi.GetResultAsync("user_cancelled_orders", apiKey,
                new Dictionary<string, string>()
                {
                    {"limit", "10"},
                    {"offset", "0"}
                });
            
            Console.WriteLine("\nОтмененные сделки пользователя:");
            foreach (var canceledOrder in testUserTradesApi.ResultList)
            {
                Console.WriteLine($"\nВалютная пара: {canceledOrder.TradeCouples}" +
                                  $"\ndate : {canceledOrder.Date}" +
                                  $"\ntype : {canceledOrder.Type}" +
                                  $"\norder_id: {canceledOrder.OrderId}" +
                                  $"\nquantity : {canceledOrder.Quantity}" +
                                  $"\nprice : {canceledOrder.Price}" +
                                  $"\namount : {canceledOrder.Amount}");
            }
            Console.ReadLine();
        }

    }
}
