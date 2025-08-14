import { cookies } from "next/headers";
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Link from "next/link";

import { getOrders } from "@/api/orders";

export default async function AdminOrdersPage() {
  const cookieStore = cookies();
  const accessToken = (await cookieStore).get("accessToken")?.value!;
  const orders = await getOrders(accessToken, "/admin/orders");

  return (
    <>
      <h1>Management Orders</h1>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Order ID</TableCell>
              <TableCell>Customer</TableCell>
              <TableCell>Address</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {orders.map((order) => (
              <TableRow key={order.id}>
                <TableCell><Link href={`/admin/orders/${order.id}`}>{order.id}</Link></TableCell>
                <TableCell>{order.customer}</TableCell>
                <TableCell>{order.address}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <p>Total Orders: {orders.length}</p>
    </>
  );
}
