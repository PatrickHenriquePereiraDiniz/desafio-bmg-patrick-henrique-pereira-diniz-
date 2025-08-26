# Desafio BMG - API de Pedidos e Produtos

Este projeto implementa uma API simples para gerenciamento de **produtos**, **pedidos** e **pagamentos**, utilizando **ASP.NET Core**, **JWT** para autenticação e **In-Memory Database** para persistência de dados utilizando .net 8.0.

---

## 🚀 Fluxo Básico

1. **Admin cria produtos**
   - Login com usuário admin (`admin@bmg.com` / `admin123`) para gerar o token JWT.
   - Criação de produtos (`POST /api/products`) protegida por role `Admin`.
   
2. **User cria pedidos**
   - Login com usuário cliente (`cliente@bmg.com` / `cliente123`) para gerar o token JWT.
   - Criação de pedidos (`POST /api/orders`) utilizando produtos já cadastrados.
   
3. **User paga pedidos**
   - Pagamento feito via `Pix` ou `Cartão` (`POST /api/orders/{orderId}/pay?method=Pix`).
   - O método de pagamento é registrado no histórico e nos comentários do pedido.
   
4. **User consulta histórico**
   - `GET /api/orders` retorna todos os pedidos do usuário, incluindo status e forma de pagamento.

---

## 🔐 Autenticação

- JWT Token é gerado no login (`POST /api/auth/login`) e deve ser usado no header `Authorization: Bearer <token>` para todos os endpoints protegidos.
- Role `Admin` controla criação/atualização/deleção de produtos.
- Role `Customer` controla criação, pagamento e histórico de pedidos.

---

## ⚙️ Decisões Técnicas

1. **JWT Authentication**
   - Permite controle de acesso por roles e identificação do usuário nos pedidos.
   
2. **Enum para Status e Pagamento**
   - `OrderStatusEnum` define status (`Pendente`, `Pago`) para padronizar o histórico.
   - `PaymentMethodEnum` define métodos de pagamento (`Pix`, `Cartão`) e é salvo nos comentários do pedido para histórico claro.
   
3. **In-Memory Database**
   - Facilita testes e prototipagem rápida.
   - Trade-off: dados não persistem entre reinícios da aplicação.

4. **Services + Repositories**
   - Separação de responsabilidades:
     - **Services:** regras de negócio.
     - **Repositories:** acesso aos dados.
   - Facilita manutenção, testes unitários e futuras alterações para banco real.

5. **DTOs para requisições**
   - Evita exposição direta das entidades do domínio.
   - Garante que apenas campos necessários sejam enviados na criação de pedidos/produtos.

---

## 🧪 Testes

- Criado teste unitário para **criação de pedidos**, garantindo:
  - Estoque atualizado corretamente.
  - Pedido criado com os itens corretos.
  
---

## 🔧 Como Rodar

1. Clonar o repositório e abrir no Visual Studio ou VS Code.
2. Configurar `appsettings.json` com chave JWT.
3. Rodar a aplicação (`dotnet run`).
4. Usar endpoints conforme fluxo básico:
   - Login Admin → Criar produtos.
   - Login User → Criar pedidos → Pagar pedidos → Consultar histórico.

---

### 📝 Credenciais Padrão

| Role  | Email              | Senha       |
|-------|------------------|------------|
| Admin | admin@bmg.com     | admin123   |
| User  | cliente@bmg.com   | cliente123 |

---

## ⚖️ Trade-offs

- **In-Memory DB:** rápido para protótipo, mas sem persistência real.
- **Pagamento simples:** apenas enum e comentário, sem integração real.
- **Fluxo básico:** pensado para demonstração; em produção, considerar:
  - Banco real.
  - Logging estruturado.
  - Validação de input mais robusta.
  - Histórico de pagamento em tabela separada.

---

