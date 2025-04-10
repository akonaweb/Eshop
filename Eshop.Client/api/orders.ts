import { CartItem } from "@/components/providers/CartProvider";

export type Cart = {
  items: CartFullItem[];
  totalPrice: number;
};

export type CartFullItem = {
  productId: number;
  productTitle: string;
  price: number;
  quantity: number;
  totalPrice: number;
};

export const getCart = async (items: CartItem[]): Promise<Cart> => {
  const result = await (
    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Order/cart`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(items),
    })
  ).json();

  return result;
};

export type Customer = {
  name: string;
  address: string;
};
export const addOrder = async (
  items: CartItem[],
  customer: Customer
): Promise<Cart> => {
  const payload = {
    items,
    customer: customer.name,
    address: customer.address,
  };

  const result = await (
    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Order`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    })
  ).json();

  return result;
};
