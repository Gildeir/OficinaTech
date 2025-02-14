## Sistema de Gestão de Oficina

### 📌 Objetivo

O objetivo deste projeto é desenvolver um sistema básico para validação de conhecimento.

### 📝 Descrição

Desenvolver um sistema para uma oficina de manutenção de veículos, automatizando e digitalizando processos. A estrutura do sistema será baseada nos seguintes conceitos:

#### 📌 Orçamentos e Peças

1. A oficina possui **orçamentos**, que contêm:

   - Um número identificador
   - A placa do veículo
   - O nome do cliente
   - As peças associadas ao orçamento

2. Cada **peça** possui um cadastro próprio com:

   - Nome
   - Preço
   - Quantidade em estoque

3. Ao adicionar uma peça a um orçamento, ela inicia no estado **"Em Espera"**.

4. Se a peça **não estiver disponível no estoque**, ela ficará liberada para compra.

5. Para comprar a peça, é necessário informar a **quantidade** e o **preço de custo**.

6. Se o preço de custo for diferente do preço cadastrado, o orçamento será atualizado.

7. Quando a peça for comprada e houver estoque disponível, será possível **realizar a entrega ao orçamento**, o que reduzirá a quantidade de estoque.

8. As **entregas** possuem um endereço vinculado, mas a oficina trabalha apenas com **CEPs dos fornecedores**, sendo necessária uma integração com uma API externa (ex: **ViaCEP**) para obter o endereço a partir do frete.

9. Toda movimentação de estoque deve ser armazenada para rastreamento.

---

### 📌 Requisitos Desejáveis

- Uso de **Docker**
- Uma **cobertura de testes consistente**
- Uso de **Design Patterns**
- **Modelagem de Dados** eficiente
- **Tratamento de erros** robusto
- Arquitetura bem pensada antes da implementação
- **Desacoplamento de componentes** (ex: separação de camadas Service e Repository)

---

### 🚀 Diferenciais

- Uso de **Docker Compose** para orquestração de serviços
