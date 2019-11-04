using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using XUnitTestMarket.Entities;

namespace XUnitTestMarket
{
    public class MainTest
    {
        public string BASE_WHOLESALE = @"http://localhost:4012/wholesaleapi/v1";
        public string WHOLESALE_POSTORDER = "/Orders";
        public string WHOLESALE_PATCHORDER = "/Budgets";
        public string BASE_SHOPKEEPER = @"http://localhost:4013/shopkeeperapi/v1";
        public string SHOPKEEPER_BUDGETS = "/Budgets";
        [Fact]
        public void Rejeitado()
        {
            //Lojista faz pedido a atacadista, caso d� certo atacadista envia a proposta de or�amento neste momento
            List<Order> orders = MockTest.getRequestOrders();
            string retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(orders), BASE_WHOLESALE + WHOLESALE_POSTORDER);
            OrderResponse orderResp = null;
            try
            {
                orderResp = JsonConvert.DeserializeObject<OrderResponse>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(orderResp == null, "OrderRequest tem que retornar o tipo OrderResponse sem erro");



            //Verificar se o atacadista enviou para o lojista a proposta de or�amento ao receber o pedido
            Budget budgetResp = null;
            retornoApi = CallApi.GetRequest(BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS+ "/"+ orderResp.budgetCode);
            try
            {
                budgetResp = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(budgetResp == null, "n�o foi poss�vel recuperar or�amento");


            //Lojista manda para o atacadista o cancelamento da ordem do or�amento
            Budget rejectBudget = null;
            RequestChangeStatus requestStatus = new RequestChangeStatus("Rejeitado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_WHOLESALE + WHOLESALE_PATCHORDER + "/" + orderResp.budgetCode,true);
            try
            {
                rejectBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(rejectBudget == null, "n�o foi poss�vel recuperar or�amento");
            Assert.True(rejectBudget.status.Equals(requestStatus.status), "O pedido n�o foi cancelado pelo atacadista");


        }

        [Fact]
        public void FluxoCompletoAprovado()
        {
            //Lojista faz pedido a atacadista, caso d� certo atacadista envia a proposta de or�amento neste momento
            List<Order> orders = MockTest.getRequestOrders();
            string retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(orders), BASE_WHOLESALE + WHOLESALE_POSTORDER);
            OrderResponse orderResp = null;
            try
            {
                orderResp = JsonConvert.DeserializeObject<OrderResponse>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(orderResp == null, "OrderRequest tem que retornar o tipo OrderResponse sem erro");



            //Verificar se o atacadista enviou para o lojista a proposta de or�amento ao receber o pedido
            Budget budgetResp = null;
            retornoApi = CallApi.GetRequest(BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode);
            try
            {
                budgetResp = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(budgetResp == null, "n�o foi poss�vel recuperar or�amento");


            //Lojista manda para o atacadista a confirma��o da ordem do or�amento
            Budget requestBudget = null;
            RequestChangeStatus requestStatus = new RequestChangeStatus("Solicitado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_WHOLESALE + WHOLESALE_PATCHORDER + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "n�o foi poss�vel recuperar or�amento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "O pedido n�o foi cancelado pelo atacadista");

            //Atacadista manda para o lojista andamento da ORDEM para: "Em Fabricacao"
            requestStatus = new RequestChangeStatus("Em Fabricacao");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "n�o foi poss�vel recuperar or�amento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


            //Atacadista manda para o lojista andamento da ORDEM para: "Finalizado"
            requestStatus = new RequestChangeStatus("Finalizado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "n�o foi poss�vel recuperar or�amento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


            //Atacadista manda para o lojista andamento da ORDEM para: "Despachado"
            requestStatus = new RequestChangeStatus("Despachado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "n�o foi poss�vel recuperar or�amento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


        }
    }
}
