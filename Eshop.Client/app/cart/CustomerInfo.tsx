import { Customer } from "@/api/orders";
import { Box, TextField, Typography } from "@mui/material";

type CustomerFunc = (prev: Customer) => Customer;
type Props = {
  customer: Customer;
  onCustomerChange: (func: CustomerFunc) => void;
};
const CustomerInfo = ({ customer, onCustomerChange }: Props) => {
  return (
    <Box
      sx={{
        p: 2,
        border: 1,
        borderColor: "divider",
        borderRadius: 2,
        width: 320,
      }}
    >
      <Typography variant="h6" gutterBottom>
        Delivery details
      </Typography>

      <TextField
        label="Customer"
        variant="outlined"
        fullWidth
        margin="dense"
        value={customer.name}
        onChange={(e) =>
          onCustomerChange((prev) => ({ ...prev, name: e.target.value }))
        }
      />
      <TextField
        label="Address"
        variant="outlined"
        fullWidth
        margin="dense"
        value={customer.address}
        onChange={(e) =>
          onCustomerChange((prev) => ({ ...prev, address: e.target.value }))
        }
      />
    </Box>
  );
};

export default CustomerInfo;
