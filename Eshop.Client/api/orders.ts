import api from "./core/api";
import apiSsr from "./core/apiSsr";
import urls from "./core/urls";
import useApiQuery from "./core/useApiQuery";

export type CartFullItem = {
  productId: number;
  productTitle: string;
  price: number;
  quantity: number;
  totalPrice: number;
};
export type Cart = {
  items: CartFullItem[];
  totalPrice: number;
};
export type CartItem = {
  productId: number;
  quantity: number;
};
export const getCart = async (items: CartItem[]): Promise<Cart> => {
  const response = await api.post(urls.order.cart, items);
  return response.data;
};
export function useCartQuery(items: CartItem[]) {
  return useApiQuery(["cart", JSON.stringify(items)], () => getCart(items), {
    enabled: items.length > 0,
  });
}

export type Customer = {
  name: string;
  address: string;
};
type AddOrderItemResonse = {
  productId: number;
  productTitle: string;
  quantity: number;
  price: number;
  totalPrice: number;
};
type AddOrderResonse = {
  id: number;
  customer: string;
  address: string;
  items: AddOrderItemResonse[];
  totalPrice: number;
};
export const addOrder = async ({
  items,
  customer,
}: {
  items: CartItem[];
  customer: Customer;
}): Promise<AddOrderResonse> => {
  const payload = {
    items,
    customer: customer.name,
    address: customer.address,
  };

  const response = await api.post(urls.order.addOrder, payload);
  return response.data;
};

export type Order = {
  id: number;
  customer: string;
  address: string;
  createdAt: Date;
};
export const getOrdersSsr = async (
  accessToken: string,
  backUrl: string
): Promise<Order[]> => {
  const response = await apiSsr(accessToken, backUrl).get(urls.order.list);
  return response.data;
};
