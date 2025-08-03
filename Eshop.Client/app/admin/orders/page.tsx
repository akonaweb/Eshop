import { cookies } from "next/headers";

import { getOrders } from "@/api/orders";

export default async function AdminOrdersPage() {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get("accessToken")?.value ?? null;
  const orders = await getOrders(accessToken);

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
