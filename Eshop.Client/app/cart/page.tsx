"use client";

import { Box, Button, Typography } from "@mui/material";
import { useCallback, useState } from "react";

import useApiMutation from "@/api/core/useApiMutation";
import { addOrder, Customer, useCartQuery } from "@/api/orders";
import Data from "@/components/Data";
import { useCartContext } from "@/components/providers/CartProvider";
import CustomerInfo from "./CustomerInfo";
import ProductsTable from "./ProductsTable";

const defaultCustomer: Customer = { name: "", address: "" };

const CartPage = () => {
  const { items, onCartEmpty } = useCartContext();
  const response = useCartQuery(items);
  const [customer, setCustomer] = useState(defaultCustomer);
  const orderMutation = useApiMutation(addOrder, {
    onSuccess: (data) => {
      alert("Order placed successfully!");
      setCustomer(defaultCustomer);
      onCartEmpty();
    },
    onError: (error) => {
      // TODO: consume error - either 400 validation or 500 - probably modal
      console.info(error);
    },
  });

  const handleBuy = useCallback(() => {
    if (!customer.name || !customer.address) {
      alert("Name & Address are required!");
      return;
    }

    orderMutation.mutate({ items, customer });
  }, [items, customer, orderMutation]);

  return (
    <Box sx={{ p: 3, width: "100%", maxWidth: 1200, mx: "auto" }}>
      <Typography variant="h5" gutterBottom>
        🛒 Cart Items
      </Typography>

      <Data response={response}>
        {(cart) => {
          if (!cart) return <div>No items</div>;
          return (
            <>
              <ProductsTable cart={cart} />

              <Box
                sx={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "flex-start",
                }}
              >
                <CustomerInfo
                  customer={customer}
                  onCustomerChange={setCustomer}
                />

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
            </>
          );
        }}
      </Data>
    </Box>
  );
};

export default CartPage;
