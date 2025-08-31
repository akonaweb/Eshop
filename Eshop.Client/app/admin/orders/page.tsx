import { getOrdersSsr } from "@/api/orders";
import { getSsrAccessToken } from "@/api/core/apiSsrUtils";
import Table from "@/components/Table";

export default async function AdminOrdersPage() {
  const accessToken = await getSsrAccessToken();
  const orders = await getOrdersSsr(accessToken, "/admin/orders");

  const columns = [
    { key: "id", label: "Order ID", isLink: true },
    { key: "customer", label: "Customer" },
    { key: "address", label: "Address" },
  ];

  return (
    <>
      <h1>Management Orders</h1>
      <Table
        data={orders}
        columns={columns}
        baseHref="/admin/orders"
        entityName="Orders"
      />
    </>
  );
}
