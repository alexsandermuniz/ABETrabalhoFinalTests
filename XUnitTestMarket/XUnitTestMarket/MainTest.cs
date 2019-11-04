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
            //Lojista faz pedido a atacadista, caso dê certo atacadista envia a proposta de orçamento neste momento
            List<Order> orders = MockTest.getRequestOrders();
            string retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(orders), BASE_WHOLESALE + WHOLESALE_POSTORDER);
            OrderResponse orderResp = null;
            try
            {
                orderResp = JsonConvert.DeserializeObject<OrderResponse>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(orderResp == null, "OrderRequest tem que retornar o tipo OrderResponse sem erro");



            //Verificar se o atacadista enviou para o lojista a proposta de orçamento ao receber o pedido
            Budget budgetResp = null;
            retornoApi = CallApi.GetRequest(BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS+ "/"+ orderResp.budgetCode);
            try
            {
                budgetResp = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(budgetResp == null, "não foi possível recuperar orçamento");


            //Lojista manda para o atacadista o cancelamento da ordem do orçamento
            Budget rejectBudget = null;
            RequestChangeStatus requestStatus = new RequestChangeStatus("Rejeitado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_WHOLESALE + WHOLESALE_PATCHORDER + "/" + orderResp.budgetCode,true);
            try
            {
                rejectBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(rejectBudget == null, "não foi possível recuperar orçamento");
            Assert.True(rejectBudget.status.Equals(requestStatus.status), "O pedido não foi cancelado pelo atacadista");


        }

        [Fact]
        public void FluxoCompletoAprovado()
        {
            //Lojista faz pedido a atacadista, caso dê certo atacadista envia a proposta de orçamento neste momento
            List<Order> orders = MockTest.getRequestOrders();
            string retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(orders), BASE_WHOLESALE + WHOLESALE_POSTORDER);
            OrderResponse orderResp = null;
            try
            {
                orderResp = JsonConvert.DeserializeObject<OrderResponse>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(orderResp == null, "OrderRequest tem que retornar o tipo OrderResponse sem erro");



            //Verificar se o atacadista enviou para o lojista a proposta de orçamento ao receber o pedido
            Budget budgetResp = null;
            retornoApi = CallApi.GetRequest(BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode);
            try
            {
                budgetResp = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(budgetResp == null, "não foi possível recuperar orçamento");


            //Lojista manda para o atacadista a confirmação da ordem do orçamento
            Budget requestBudget = null;
            RequestChangeStatus requestStatus = new RequestChangeStatus("Solicitado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_WHOLESALE + WHOLESALE_PATCHORDER + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "não foi possível recuperar orçamento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "O pedido não foi cancelado pelo atacadista");

            //Atacadista manda para o lojista andamento da ORDEM para: "Em Fabricacao"
            requestStatus = new RequestChangeStatus("Em Fabricacao");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "não foi possível recuperar orçamento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


            //Atacadista manda para o lojista andamento da ORDEM para: "Finalizado"
            requestStatus = new RequestChangeStatus("Finalizado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "não foi possível recuperar orçamento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


            //Atacadista manda para o lojista andamento da ORDEM para: "Despachado"
            requestStatus = new RequestChangeStatus("Despachado");
            retornoApi = CallApi.PostRequest(JsonConvert.SerializeObject(requestStatus), BASE_SHOPKEEPER + SHOPKEEPER_BUDGETS + "/" + orderResp.budgetCode, true);
            try
            {
                requestBudget = JsonConvert.DeserializeObject<Budget>(retornoApi);
            }
            catch (Exception ex) { }

            Assert.False(requestBudget == null, "não foi possível recuperar orçamento");
            Assert.True(requestBudget.status.Equals(requestStatus.status), "A notificacao nao foi recebida pelo lojista");


        }
    }
}
