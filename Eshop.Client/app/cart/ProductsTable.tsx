"use client";

import DeleteIcon from "@mui/icons-material/Delete";
import {
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from "@mui/material";

import { Cart } from "@/api/orders";
import { useCartContext } from "@/components/providers/CartProvider";

type Props = {
  cart: Cart;
};
const ProductsTable = ({ cart }: Props) => {
  const { onProductUpdate, onProductRemove } = useCartContext();

  return (
    <>
      <TableContainer component={Paper} elevation={3} sx={{ mb: 3 }}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>
                <b>Product</b>
              </TableCell>
              <TableCell>
                <b>Quantity</b>
              </TableCell>
              <TableCell>
                <b>Price (€)</b>
              </TableCell>
              <TableCell>
                <b>Total (€)</b>
              </TableCell>
              <TableCell align="center"></TableCell>
            </TableRow>
          </TableHead>

          <TableBody>
            {cart.items.map((item) => (
              <TableRow key={item.productId}>
                <TableCell>{item.productTitle}</TableCell>
                <TableCell>
                  <TextField
                    type="number"
                    variant="outlined"
                    size="small"
                    value={item.quantity}
                    onChange={(e) =>
                      onProductUpdate({
                        productId: item.productId,
                        quantity: Number(e.target.value),
                      })
                    }
                    sx={{ width: 70 }}
                  />
                </TableCell>
                <TableCell>{item.price.toFixed(2)}</TableCell>
                <TableCell>{(item.price * item.quantity).toFixed(2)}</TableCell>
                <TableCell align="center">
                  <IconButton
                    color="error"
                    onClick={() => onProductRemove(item.productId)}
                  >
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Typography variant="h6" sx={{ textAlign: "right", mb: 3 }}>
        <b>Total: {cart.totalPrice} €</b>
      </Typography>
    </>
  );
};

export default ProductsTable;
