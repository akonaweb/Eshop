"use client";

import {
  Backdrop,
  Box,
  Button,
  CircularProgress,
  Typography,
} from "@mui/material";
import { useCallback, useEffect, useState } from "react";

import { addOrder, Cart, Customer, getCart } from "@/api/orders";
import { useCartContext } from "@/components/providers/CartProvider";
import CustomerInfo from "./CustomerInfo";
import ProductsTable from "./ProductsTable";

const defaultCustomer: Customer = { name: "", address: "" };

const CartPage = () => {
  const { items, onCartEmpty } = useCartContext();
  const [cart, setCart] = useState<Cart>();
  const [customer, setCustomer] = useState(defaultCustomer);

  useEffect(() => {
    const fetch = async () => {
      if (!items.length) {
        setCart({ items: [], totalPrice: 0 });
        return;
      }
      const result = await getCart(items);
      setCart(result);
    };

    if (!items) return;
    fetch();
  }, [items]);

  const handleBuy = useCallback(async () => {
    if (!customer.name || !customer.address) {
      alert("Name & Address are required!");
      return;
    }

    await addOrder(items, customer);

    alert(
      `Order for customer: ${customer.name} will be delivered to the address: ${customer.address}`
    );

    setCustomer(defaultCustomer);
    onCartEmpty();
  }, [items, customer, onCartEmpty]);

  if (!cart)
    return (
      <Backdrop open>
        <CircularProgress color="inherit" />
      </Backdrop>
    );

  return (
    <Box sx={{ p: 3, width: "100%", maxWidth: 1200, mx: "auto" }}>
      <Typography variant="h5" gutterBottom>
        🛒 Cart Items
      </Typography>

      <ProductsTable cart={cart} />

      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "flex-start",
        }}
      >
        <CustomerInfo customer={customer} onCustomerChange={setCustomer} />

        <Button
          variant="contained"
          color="primary"
          onClick={handleBuy}
          sx={{ height: 40 }}
          disabled={!cart.items.length}
        >
          Buy
        </Button>
      </Box>
    </Box>
  );
};

export default CartPage;
