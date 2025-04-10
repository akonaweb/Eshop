"use client";

import { Button, TextField } from "@mui/material";
import { ChangeEvent, useCallback, useState } from "react";

import { useCartContext } from "@/components/providers/CartProvider";

type Props = {
  productId: number;
};
const QuantityAndBuy = ({ productId }: Props) => {
  const [quantity, setQuantity] = useState(1);
  const { onProductAdd } = useCartContext();

  const handleChangeQuantity = useCallback(
    (e: ChangeEvent<HTMLInputElement>) => {
      const value = Number(e.target.value);
      setQuantity(value <= 1 ? 1 : value);
    },
    []
  );

  const handleBuy = useCallback(() => {
    onProductAdd({ productId, quantity });
  }, [productId, quantity, onProductAdd]);

  return (
    <>
      <TextField
        type="number"
        label="Quantity"
        variant="outlined"
        size="small"
        fullWidth
        margin="normal"
        value={quantity}
        onChange={handleChangeQuantity}
      />

      <Button variant="contained" color="primary" fullWidth onClick={handleBuy}>
        Buy
      </Button>
    </>
  );
};

export default QuantityAndBuy;
