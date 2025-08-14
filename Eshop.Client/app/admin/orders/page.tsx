import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Link from "next/link";

import { getSsrAccessToken } from "@/api/core/apiSsrUtils";
import { getOrdersSsr } from "@/api/orders";

export default async function AdminOrdersPage() {
  const accessToken = await getSsrAccessToken();
  const orders = await getOrdersSsr(accessToken, "/admin/orders");

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
                <TableCell>
                  <Link href={`/admin/orders/${order.id}`}>{order.id}</Link>
                </TableCell>
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
