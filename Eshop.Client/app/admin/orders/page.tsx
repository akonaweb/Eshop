import { cookies } from "next/headers";

import { getOrders } from "@/api/orders";

export default async function AdminOrdersPage() {
  const cookieStore = cookies();
  const accessToken = (await cookieStore).get("accessToken")?.value!;
  const orders = await getOrders(accessToken, "/admin/orders");

  return (
    <>
      <h1>Management Orders</h1>

      <ul>
        {orders.map((x) => (
          <li key={x.id}>
            {x.id} - {x.customer} - {x.address}
          </li>
        ))}
      </ul>
    </>
  );
}
